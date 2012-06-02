/*
 * Copyright (c) Marcus Kirsch (aka Marck). Contributors, http://aurora-sim.org/, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using Aurora.Framework;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using OpenSim.Services.Interfaces;

namespace AuroraWeb.Data.MySQL
{
    /// <summary>
    /// An Inventory Interface to the SQLite database
    /// </summary>
    public class MySQLInventoryStore : IInventoryService
    {
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
}