﻿/**
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
using System.Globalization;
using OpenMetaverse;

using OpenSim.Services.Interfaces;

namespace AuroraWeb
{
    public partial class Services
    {
        public string UserManagementGetRequest(AuroraWeb.Environment env)
        {
            m_log.DebugFormat("[AuroraWeb]: UserManagementGetRequest");
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                env.Flags = Flags.IsLoggedIn | Flags.IsAdmin;
                env.State = State.UserSearchForm;
                env.Data = GetUserList(env, m_PendingIdentifier);
                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }
            
            return m_WebApp.ReadFile(env, "index.html");
        }

        public string UserSearchPostRequest(AuroraWeb.Environment env, string terms)
        {
            m_log.DebugFormat("[AuroraWeb]: UserSearchPostRequest");
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                if (terms != string.Empty)
                {
                    env.Session = sinfo;
                    env.Flags = Flags.IsLoggedIn | Flags.IsAdmin;
                    env.State = State.UserSearchFormResponse;
                    // Put the list in the environment
                    var accounts = m_UserAccountService.GetActiveAccounts(UUID.Zero, terms, m_PendingIdentifier);
                    env.Data = Objectify<UserAccount>(null);

                    return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
                }
                else
                    return UserManagementGetRequest(env);
            }

            return m_WebApp.ReadFile(env, "index.html");
        }

        public string UserEditGetRequest(AuroraWeb.Environment env, UUID userID)
        {
            m_log.DebugFormat("[AuroraWeb]: UserEditGetRequest {0}", userID);
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                env.Flags = Flags.IsLoggedIn | Flags.IsAdmin ;
                env.State = State.UserEditForm;
                UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, userID);
                if (account != null)
                {
                    List<object> loo = new List<object>();
                    loo.Add(account);
                    env.Data = loo;
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }
            
            return m_WebApp.ReadFile(env, "index.html");
        }

        public string UserEditPostRequest(Environment env, UUID userID, string first, string last, string email, int level, int flags, string title)
        {
            m_log.DebugFormat("[AuroraWeb]: UserEditPostRequest {0}", userID);
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, userID);
                if (account != null)
                {
                    // Update the account
                    //account.FirstName = first;
                    //account.LastName = last;
                    account.Email = email;
                    account.UserFlags = flags;
                    account.UserLevel = level;
                    account.UserTitle = title;
                    m_UserAccountService.StoreUserAccount(account);

                    env.Flags = Flags.IsLoggedIn | Flags.IsAdmin;
                    NotifyWithoutButton(env, _("The account has been updated.", env));
                    m_log.DebugFormat("[AuroraWeb]: Updated account for user {0}", account.Name);
                }
                else
                {
                    NotifyWithoutButton(env, _("The account does not exist.", env));
                    m_log.DebugFormat("[AuroraWeb]: Attempt at updating an inexistent account");
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");

        }

        public string UserEditPostRequest(AuroraWeb.Environment env, UUID userID, string password)
        {
            m_log.DebugFormat("[AuroraWeb]: UserEditPostRequest (password) {0}", userID);
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, userID);
                if (account != null)
                {
                    if (password != string.Empty)
                        m_AuthenticationService.SetPassword(account.PrincipalID, "UserAccount",password);

                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    NotifyWithoutButton(env, _("The account has been updated.", env));
                    m_log.DebugFormat("[AuroraWeb]: Updated account for user {0}", account.Name);
                }
                else
                {
                    NotifyWithoutButton(env, _("The account does not exist.", env));
                    m_log.DebugFormat("[AuroraWeb]: Attempt at updating an inexistent account");
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");

        }


        public string UserActivateGetRequest(AuroraWeb.Environment env, UUID userID)
        {
            m_log.DebugFormat("[AuroraWeb]: UserActivateGetRequest {0}", userID);
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                env.Flags = Flags.IsLoggedIn | Flags.IsAdmin;
                NotifyWithoutButton(env, _("The account has been activated.", env));
                UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, userID);
                if (account != null)
                {
                    // Remove pending identifier in name
                    //account.FirstName = account.FirstName.Replace(m_PendingIdentifier, "");

                    // Retrieve saved user data from serviceURLs and set them back to normal
                    string password = (string)account.ServiceURLs["Password"];
                    account.ServiceURLs.Remove("Password");

                    Object value;
                    account.ServiceURLs.TryGetValue("Avatar", out value);
                    account.ServiceURLs.Remove("Avatar");
                    string avatarType = (string)value;

                    CultureInfo[] languages = null;
                    if (account.ServiceURLs.TryGetValue("Language", out value))
                        languages = Localization.GetLanguageInfo((string)value);
                    account.ServiceURLs.Remove("Language");

                    // Save changes to user account
                    m_UserAccountService.StoreUserAccount(account);

                    // Create the inventory
                    m_InventoryService.CreateUserInventory(account.PrincipalID, true);

                    // Set the password
                    m_AuthenticationService.SetPassword(account.PrincipalID,"UserAccount", password);

                    // Set the avatar
                    if (avatarType != null)
                    {
                        SetAvatar(env, account.PrincipalID, avatarType);
                    }

                    if (account.Email != string.Empty)
                    {
                        string message = string.Format("{0}\n\n{1} {2}\n{3} {4}\n\n{5} {6}",
                            _("Your account has been activated.", languages),
                            _("First name:", languages), account.FirstName,
                            _("Last name:", languages), account.LastName,
                            _("LoginURI:", languages), m_WebApp.LoginURL);
                        SendEMail(account.Email, _("Account activated", languages), message);
                    }
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");
        }

        public string UserDeleteGetRequest(AuroraWeb.Environment env, UUID userID)
        {
            m_log.DebugFormat("[AuroraWeb]: UserDeleteGetRequest {0}", userID);
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                env.Flags = Flags.IsLoggedIn | Flags.IsAdmin;
                env.State = State.UserDeleteForm;
                UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, userID);
                if (account != null)
                {
                    List<object> loo = new List<object>();
                    loo.Add(account);
                    env.Data = loo;
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");
        }


        public string UserDeletePostRequest(AuroraWeb.Environment env, UUID userID)
        {
            m_log.DebugFormat("[AuroraWeb]: UserDeletePostRequest {0}", userID);
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, userID);
                if (account != null)
                {

                    m_UserAccountService.DeleteAccount(UUID.Zero, userID);
                    m_InventoryService.DeleteUserInventory(userID);

                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    NotifyWithoutButton(env, _("The account has been deleted.", env));
                    m_log.DebugFormat("[AuroraWeb]: Deleted account for user {0}", account.Name);
                }
                else
                {
                    NotifyWithoutButton(env, _("Unable to delete account because it does not exist.", env));
                    m_log.DebugFormat("[AuroraWeb]: Attempt at deleting an inexistent account");
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");
        }
    }
}
