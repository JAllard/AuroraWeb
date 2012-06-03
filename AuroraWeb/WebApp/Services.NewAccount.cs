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

using System.Collections.Generic;
using System.Linq;
using AuroraWeb.LoginService;
using OpenMetaverse.StructuredData;
using OpenMetaverse;

using Aurora.Framework;
using OpenSim.Services.Interfaces;

namespace AuroraWeb
{
    public partial class Services
    {
        public string NewAccountGetRequest(AuroraWeb.Environment env)
        {
            m_log.DebugFormat("[AuroraWeb]: NewAccountGetRequest");
            Request request = env.Request;

            env.State = State.NewAccountForm;
            env.Data = GetDefaultAvatarSelectionList();

            return m_WebApp.ReadFile(env, "index.html");
        }

        public string NewAccountPostRequest(AuroraWeb.Environment env, string first, string last, string email, string password, string password2, string avatarType)
        {
            if (!m_WebApp.IsInstalled)
            {
                m_log.DebugFormat("[AuroraWeb]: warning: someone is trying to access NewAccountPostRequest and Wifi isn't installed!");
                return m_WebApp.ReadFile(env, "index.html");
            }


            m_log.DebugFormat("[AuroraWeb]: NewAccountPostRequest");
            Request request = env.Request;

            if ((password != string.Empty) && (password == password2) && (first != string.Empty) && (last != string.Empty))
            {
                UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, first, last);
                if (account == null)
                    account = m_UserAccountService.GetUserAccount(UUID.Zero, m_PendingIdentifier + first, last);
                if (account == null)
                {
                    Dictionary<string, object> urls = new Dictionary<string, object>();

                    if (m_WebApp.AccountConfirmationRequired)
                    {
                        //attach pending identifier to first name
                        first = m_PendingIdentifier + first;
                        // Store the password temporarily here
                        urls["Password"] = password;
                        urls["Avatar"] = avatarType;
                        if (env.LanguageInfo != null)
                            urls["Language"] = Localization.LanguageInfoToString(env.LanguageInfo);
                    }

                    // Create the account
                    string name = first + " " + last;
                    account = new UserAccount(UUID.Zero, name, email);
                    account.ServiceURLs = urls;
                    account.UserTitle = "Local User";

                    m_UserAccountService.StoreUserAccount(account);

                    string notification = _("Your account has been created.", env);
                    if (!m_WebApp.AccountConfirmationRequired)
                    {
                        // Create the inventory
                        m_InventoryService.CreateUserInventory(account.PrincipalID, true);

                        // Store the password
                        m_AuthenticationService.SetPassword(account.PrincipalID, "",password);

                        // Set avatar
                        SetAvatar(env, account.PrincipalID, avatarType);
                    }
                    else if (m_WebApp.AdminEmail != string.Empty)
                    {
                        string message = string.Format(
                            _("New account {0} {1} created in {2} is awaiting your approval.",
                            m_WebApp.AdminLanguage),
                            first, last, m_WebApp.GridName);
                        message += "\n\n" + m_WebApp.WebAddress + "/wifi";
                        SendEMail(
                            m_WebApp.AdminEmail,
                            _("Account awaiting approval", m_WebApp.AdminLanguage),
                            message);
                        notification = _("Your account awaits administrator approval.", env);
                    }

                    AuroraWeb.Services.NotifyWithoutButton(env, notification);
                    m_log.DebugFormat("[AuroraWeb]: Created account for user {0}", account.Name);
                }
                else
                {
                    m_log.DebugFormat("[AuroraWeb]: Attempt at creating an account that already exists");
                    env.State = State.NewAccountForm;
                    env.Data = GetDefaultAvatarSelectionList();
                }
            }
            else
            {
                m_log.DebugFormat("[AuroraWeb]: did not create account because of password and/or user name problems");
                env.State = State.NewAccountForm;
                env.Data = GetDefaultAvatarSelectionList();
            }

            return m_WebApp.ReadFile(env, "index.html");

        }

        private void SetAvatar(Environment env, UUID newUser, string avatarType)
        {
            UserAccount account = null;
            string[] parts = null;

            AuroraWeb.Avatar defaultAvatar = m_WebApp.DefaultAvatars.FirstOrDefault(av => av.Type.Equals(avatarType));
            if (defaultAvatar.Name != null)
                parts = defaultAvatar.Name.Split(new char[] { ' ' });

            if (parts == null || (parts != null && parts.Length != 2))
                return;

            account = m_UserAccountService.GetUserAccount(UUID.Zero, parts[0], parts[1]);
            if (account == null)
            {
                m_log.WarnFormat("[AuroraWeb]: Tried to get avatar of account {0} {1} but that account does not exist", parts[0], parts[1]);
                return;
            }

            AvatarData avatar = m_AvatarService.GetAvatar(account.PrincipalID);

            if (avatar == null)
            {
                m_log.WarnFormat("[AuroraWeb]: Avatar of account {0} {1} is null", parts[0], parts[1]);
                return;
            }

            m_log.DebugFormat("[AuroraWeb]: Creating {0} avatar (account {1} {2})", avatarType, parts[0], parts[1]);

            // Get and replicate the attachments
            // and put them in a folder named after the avatar type under Clothing
            string folderName = _("Default Avatar", env) + " " + _(defaultAvatar.PrettyType, env);
            UUID defaultFolderID = CreateDefaultAvatarFolder(newUser, folderName.Trim());

            if (defaultFolderID != UUID.Zero)
            {
                Dictionary<string, string> attchs = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> _kvp in avatar.Data)
                {
                    if (_kvp.Value != null)
                    {
                        string itemID = CreateItemFrom(_kvp.Value, newUser, defaultFolderID);
                        if (itemID != string.Empty)
                            attchs[_kvp.Key] = itemID;
                    }
                }

                foreach (KeyValuePair<string, string> _kvp in attchs)
                    avatar.Data[_kvp.Key] = _kvp.Value;

                m_AvatarService.SetAvatar(newUser, avatar);
            }
            else
                m_log.Debug("[AuroraWeb]: could not create folder " + folderName);

            // Set home and last location for new account
            // Config setting takes precedence over home location of default avatar
            PrepareHomeLocation();
            UUID homeRegion = AuroraWeb.Avatar.HomeRegion;
            Vector3 position = AuroraWeb.Avatar.HomeLocation;
            Vector3 lookAt = new Vector3();
            if (homeRegion == UUID.Zero)
            {
                LLLoginService.GridUserInfo userInfo = m_GridUserService.GetGridUserInfo(account.PrincipalID.ToString());
                if (userInfo != null)
                {
                    homeRegion = userInfo.HomeRegionID;
                    position = userInfo.HomePosition;
                    lookAt = userInfo.HomeLookAt;
                }
            }
            if (homeRegion != UUID.Zero)
            {
                m_GridUserService.SetHome(newUser.ToString(), homeRegion, position, lookAt);
                m_GridUserService.SetLastPosition(newUser.ToString(), UUID.Zero, homeRegion, position, lookAt);
            }
        }

        private UUID CreateDefaultAvatarFolder(UUID newUserID, string folderName)
        {
            var clothing = m_InventoryService.GetFolderForType(newUserID, InventoryType.Wearable,  AssetType.Clothing);
            if (clothing == null)
            {
                clothing = m_InventoryService.GetRootFolder(newUserID);
                if (clothing == null)
                    return UUID.Zero;
            }

            InventoryFolderBase defaultAvatarFolder = new InventoryFolderBase(UUID.Random(), folderName, newUserID, UUID.Zero);
            defaultAvatarFolder.Version = 1;
            defaultAvatarFolder.Type = (short)AssetType.Clothing;

            if (!m_InventoryService.AddFolder(clothing))
                m_log.DebugFormat("[AuroraWeb]: Failed to store {0} folder", folderName);

            return defaultAvatarFolder.ID;
        }

        private string CreateItemFrom(string itemID, UUID newUserID, UUID defaultFolderID)
        {
            InventoryItemBase item = new InventoryItemBase();
            item.Owner = newUserID;
            OSDArray retrievedItem = null;
            InventoryItemBase copyItem = null;

            UUID uuid = UUID.Zero;
            if (UUID.TryParse(itemID, out uuid))
            {
                item.ID = uuid;
                retrievedItem = m_InventoryService.GetItem((UUID) itemID);
                if (retrievedItem != null)
                {
                    copyItem = CopyFrom(retrievedItem, newUserID, defaultFolderID);
                    m_InventoryService.AddItem(itemID);
                    return copyItem.ID.ToString();
                }
            }

            return string.Empty;
        }

        private InventoryItemBase CopyFrom(OSDArray from, UUID newUserID, UUID defaultFolderID)
        {
            //InventoryItemBase to = (InventoryItemBase)from.Clone();
            //to.Owner = newUserID;
            //to.Folder = defaultFolderID;
            //to.ID = UUID.Random();

            //return to;
            return null;
        }
    }
}
