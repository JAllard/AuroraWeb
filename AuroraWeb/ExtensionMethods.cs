﻿/**
 * Copyright (c) Crista Lopes (aka Diva), Ryan Hsu, JAllard, Enrico Nirvana. All rights reserved.
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

using System.Collections.Generic;
using System.Linq;
using Aurora.Framework;
using OpenSim.Services.Interfaces;
using AuroraWeb.OpenSimServices;
using AuroraWeb.WifiScript;
using GridRegion = OpenSim.Services.Interfaces.GridRegion;

namespace AuroraWeb
{
    public static class ExtensionMethods
    {
        public static string GetX(this GridRegion gr, IEnvironment env)
        {
            int result = gr.RegionLocX / (int)Constants.RegionSize;
            return result.ToString();
        }

        public static string GetY(this GridRegion gr, IEnvironment env)
        {
            int result = gr.RegionLocY / (int)Constants.RegionSize;
            return result.ToString();
        }

        public static string GetInventoryChildren(this InventoryTreeNode node, IEnvironment env)
        {
            //Console.WriteLine(" GetInventoryChildren --> " + node.Name);
            if (node.Children == null)
                return string.Empty;
        
            // else it's a folder
            string invListStr = string.Empty;
            Environment env2 = (Environment)env;
            //Console.WriteLine(" GetInventoryChildren --> child count " + node.Children.Count);
            foreach (InventoryTreeNode child in node.Children)
            {
                // Create a new environment
                Environment newEnv = new Environment(env2.Request);
                newEnv.Flags = env2.Flags;
                newEnv.Session = env2.Session;
                newEnv.State = env2.State;
                newEnv.Data = new List<object>();
                newEnv.Data.Add(child);

                //Console.WriteLine("-------- " + child.Name + " ------");
                invListStr += WebApp.WebAppInstance.ReadFile(newEnv, "inventorylistitem.html");
            }

            return invListStr;
        }

        public static string Indent(this InventoryTreeNode node, IEnvironment env)
        {
            string indent = string.Empty;
            for (int i = 0; i < node.Depth; i++)
                indent += "&nbsp;&nbsp;&nbsp;&nbsp;";

            if (node.Children != null)
                indent += "&raquo; ";
            return indent;
        }

        public static string GetNodeName(this InventoryTreeNode node, IEnvironment env)
        {
            if (node.Depth <= 2 && node.IsFolder())
                return Localization.Translate(env, node.Name); // translate system folder names
            return node.Name;
        }

        public static string GetFolders(this InventoryTreeNode node, IEnvironment env)
        {
            Environment env2 = (Environment)env;
            if (env2.Data == null || env2.Data.Count == 0)
                return string.Empty;

            string result = string.Empty;
            foreach (object obj in env2.Data)
            {
                try
                {
                    InventoryTreeNode n = (InventoryTreeNode)obj;
                    if (n.IsFolder())
                    {
                        // first node is the very top root, UUID.Zero
                        foreach (InventoryTreeNode child in n.Children)
                        {
                            string name = Localization.Translate(env, child.Name);
                            if (name.Length > 40)
                                name = name.Substring(0, 40);
                            result += "<option value=\"" + child.ID + "\">" + name + "</option>\n";
                            foreach (InventoryTreeNode gchild in child.Children.Where(c => c.IsFolder()))
                            {
                                string childName = name + "/" + Localization.Translate(env, gchild.Name);
                                if (childName.Length > 40)
                                    childName = childName.Substring(0, 40);
                                result += "<option value=\"" + gchild.ID + "\">" + childName + "</option>\n";
                            }
                        }
                    }
                }
                catch { /* */ }
            }

            return result;
        }

        public static string GetAvatarPreselection(this Avatar avatar, IEnvironment env)
        {
            if (avatar.isDefault)
                return "checked=\"checked\"";

            return string.Empty;
        }

        public static string GetAccountCreated(this UserAccount account, IEnvironment env)
        {
            Environment env2 = (Environment)env;
            if (env2.Data == null || env2.Data.Count == 0)
                return string.Empty;

            // Return date and time in a fomat that JavaScript can easily parse
            return Util.ToDateTime(account.Created).ToString("R");
        }

        public static string GetHyperlinkCommands(this RegionInfo region, IEnvironment env)
        {
            Environment env2 = (Environment)env;
            if (env2.Session.Account.UserLevel < WebApp.WebAppInstance.AdminUserLevel)
                if ((env2.Flags & Flags.AllowHyperlinks) == 0 ||
                    (region.RegionOwnerID != env2.Session.Account.PrincipalID))
                    return "&nbsp;&nbsp;&nbsp;";

            string commandDelete = string.Format("<a href=\"{0}{1}\">{2}</a>",
                "/wifi/linkregion/delete/", region.RegionID, _("delete", env));
            return commandDelete;
        }

        private static string _(string textId, IEnvironment env)
        {
            return Localization.Translate(env, textId);
        }
    }
}
