/**
 * Copyright (c) Crista Lopes (aka Diva), JAllard, Enrico Nirvana. All rights reserved.
 * Contributors, http://aurora-sim.org/, http://opensimulator.org/
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 *     * Redistributions of source code must retain the above copyright notice, 
 *       this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright notice, 
 *       this list of conditions and the following disclaimer in the documentation 
 *       and/or other materials provided with the distribution.
 *     * Neither the name of the Organizations nor the names of Individual
 *       Contributors may be used to endorse or promote products derived from 
 *       this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
 * THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, 
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using Aurora.Framework;
using Nini.Config;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using log4net;

using AuroraWeb.Utils;

using OpenSim.Services;
using OpenSim.Services.Interfaces;
using OpenSim.Services.InventoryService;

namespace AuroraWeb.OpenSimServices
{
    public class InventoryService : IInventoryService
    {
        public InventoryService(IConfigSource config)
            : base()
        {
        }
        protected IXInventoryData m_Database;
        public class XInventoryFolder
        {
            public string folderName;
            public int type;
            public int version;
            public UUID folderID;
            public UUID agentID;
            public UUID parentFolderID;
        }
        public class XInventoryItem
        {
            public UUID assetID;
            public int assetType;
            public string inventoryName;
            public string inventoryDescription;
            public int inventoryNextPermissions;
            public int inventoryCurrentPermissions;
            public int invType;
            public string creatorID;
            public int inventoryBasePermissions;
            public int inventoryEveryOnePermissions;
            public int salePrice;
            public int saleType;
            public int creationDate;
            public UUID groupID;
            public int groupOwned;
            public int flags;
            public UUID inventoryID;
            public UUID avatarID;
            public UUID parentFolderID;
            public int inventoryGroupPermissions;
        }
        public interface IXInventoryData
        {
            XInventoryFolder[] GetFolders(string[] fields, string[] vals);
            XInventoryItem[] GetItems(string[] fields, string[] vals);

            bool StoreFolder(XInventoryFolder folder);
            bool StoreItem(XInventoryItem item);

            /// <summary>
            /// Delete folders where field == val
            /// </summary>
            /// <param name="field"></param>
            /// <param name="val"></param>
            /// <returns>true if the delete was successful, false if it was not</returns>
            bool DeleteFolders(string field, string val);

            /// <summary>
            /// Delete folders where field1 == val1, field2 == val2...
            /// </summary>
            /// <param name="fields"></param>
            /// <param name="vals"></param>
            /// <returns>true if the delete was successful, false if it was not</returns>
            bool DeleteFolders(string[] fields, string[] vals);

            /// <summary>
            /// Delete items where field == val
            /// </summary>
            /// <param name="field"></param>
            /// <param name="val"></param>
            /// <returns>true if the delete was successful, false if it was not</returns>
            bool DeleteItems(string field, string val);

            /// <summary>
            /// Delete items where field1 == val1, field2 == val2...
            /// </summary>
            /// <param name="fields"></param>
            /// <param name="vals"></param>
            /// <returns>true if the delete was successful, false if it was not</returns>
            bool DeleteItems(string[] fields, string[] vals);

            bool MoveItem(string id, string newParent);
            XInventoryItem[] GetActiveGestures(UUID principalID);
            int GetAssetPermissions(UUID principalID, UUID assetID);
        }
        public void DeleteUserInventory(UUID userID)
        {
            m_Database.DeleteFolders("agentID", userID.ToString());
            m_Database.DeleteItems("avatarID", userID.ToString());
        }

        public InventoryTreeNode GetInventoryTree(UUID userID)
        {
            XInventoryFolder[] folders = m_Database.GetFolders(new string[] { "agentID" }, new string[] { userID.ToString() });
            XInventoryItem[] items = m_Database.GetItems(new string[] { "avatarID" }, new string[] { userID.ToString() });

            List<XInventoryFolder> folderList = null;
            if (folders != null)
                folderList = new List<XInventoryFolder>(folders);
            else
                folderList = new List<XInventoryFolder>();

            List<XInventoryItem> itemList = null;
            if (items != null)
                itemList = new List<XInventoryItem>(items);
            else
                itemList = new List<XInventoryItem>();

            InventoryTreeNode root = new InventoryTreeNode(UUID.Zero, String.Empty, AssetType.Unknown, 0, true);
            FillIn(root, folderList, itemList);

            //Dump(root, string.Empty);
            return root;
            
        }

        private void FillIn(InventoryTreeNode root, List<XInventoryFolder> folders, List<XInventoryItem> items)
        {
            List<XInventoryItem> childrenItems = items.FindAll(delegate(XInventoryItem i) { return i.parentFolderID == root.ID; });
            List<XInventoryFolder> childrenFolders = folders.FindAll(delegate(XInventoryFolder f) { return f.parentFolderID == root.ID; });

            if (childrenItems != null)
                foreach (XInventoryItem it in childrenItems)
                {
                    root.Children.Add(new InventoryTreeNode(it.inventoryID, it.inventoryName, (AssetType)it.invType, root.Depth + 1, false));
                    items.Remove(it);
                }
            if (childrenFolders != null)
                foreach (XInventoryFolder fo in childrenFolders)
                {
                    InventoryTreeNode node = new InventoryTreeNode(fo.folderID, fo.folderName, (AssetType)fo.type, root.Depth + 1, true);
                    root.Children.Add(node);
                    folders.Remove(fo);
                    // Recurse
                    FillIn(node, folders, items);
                }
        }

        private void Dump(InventoryTreeNode node, string _ident)
        {
            Console.WriteLine(_ident + node.Name);
            if (node.Children != null)
            {
                foreach (InventoryTreeNode n in node.Children)
                    Dump(n, _ident + "\t");
            }
        }

        public bool CreateUserInventory(UUID user, bool createDefaultItems)
        {
            throw new NotImplementedException();
        }

        public bool CreateUserRootFolder(UUID principalID)
        {
            throw new NotImplementedException();
        }

        public List<InventoryFolderBase> GetInventorySkeleton(UUID userId)
        {
            throw new NotImplementedException();
        }

        public InventoryFolderBase GetRootFolder(UUID userID)
        {
            throw new NotImplementedException();
        }

        public InventoryFolderBase GetFolderByOwnerAndName(UUID userID, string FolderName)
        {
            throw new NotImplementedException();
        }

        public List<InventoryFolderBase> GetRootFolders(UUID userID)
        {
            throw new NotImplementedException();
        }

        public InventoryFolderBase GetFolderForType(UUID userID, InventoryType invType, AssetType type)
        {
            throw new NotImplementedException();
        }

        public InventoryCollection GetFolderContent(UUID userID, UUID folderID)
        {
            throw new NotImplementedException();
        }

        public List<InventoryFolderBase> GetFolderFolders(UUID userID, UUID folderID)
        {
            throw new NotImplementedException();
        }

        public List<InventoryItemBase> GetFolderItems(UUID userID, UUID folderID)
        {
            throw new NotImplementedException();
        }

        public bool AddFolder(InventoryFolderBase folder)
        {
            throw new NotImplementedException();
        }

        public bool UpdateFolder(InventoryFolderBase folder)
        {
            throw new NotImplementedException();
        }

        public bool MoveFolder(InventoryFolderBase folder)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFolders(UUID userID, List<UUID> folderIDs)
        {
            throw new NotImplementedException();
        }

        public bool ForcePurgeFolder(InventoryFolderBase folder)
        {
            throw new NotImplementedException();
        }

        public bool PurgeFolder(InventoryFolderBase folder)
        {
            throw new NotImplementedException();
        }

        public bool AddItem(InventoryItemBase item)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(InventoryItemBase item)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAssetIDForItem(UUID itemID, UUID assetID)
        {
            throw new NotImplementedException();
        }

        public bool MoveItems(UUID ownerID, List<InventoryItemBase> items)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItems(UUID userID, List<UUID> itemIDs)
        {
            throw new NotImplementedException();
        }

        public InventoryItemBase GetItem(InventoryItemBase item)
        {
            throw new NotImplementedException();
        }

        public InventoryFolderBase GetFolder(InventoryFolderBase folder)
        {
            throw new NotImplementedException();
        }

        public List<InventoryItemBase> GetActiveGestures(UUID userId)
        {
            throw new NotImplementedException();
        }

        public OSDArray GetLLSDFolderItems(UUID principalID, UUID folderID)
        {
            throw new NotImplementedException();
        }

        public OSDArray GetItem(UUID itemID)
        {
            throw new NotImplementedException();
        }
    }

    public class InventoryTreeNode
    {
        public UUID ID;
        public string Name;
        public AssetType Type;
        public int Depth;
        // null means item; non-null means folder
        public List<InventoryTreeNode> Children;

        public InventoryTreeNode(UUID _id, string _name, AssetType _type, int _depth, bool isFolder)
        {
            ID = _id;
            Name = _name;
            Type = _type;
            Depth = _depth;
            if (isFolder)
                Children = new List<InventoryTreeNode>();
        }

        public bool IsFolder()
        {
            return !(Children == null);
        }

        public override string ToString()
        {
            return "Name: " + Name;
        }

        public string NodeType
        {
            get
            {
                if (IsFolder())
                    return "folder";

                return "item";
            }
        }

        [Translate]
        public string ItemType
        {
            get
            {
                if (Children == null)
                    return Type.ToString();

                return string.Empty;
            }
        }


    }
}
