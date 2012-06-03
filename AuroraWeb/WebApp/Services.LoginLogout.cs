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

using System.Collections.Generic;
using Aurora.Framework;
using OpenMetaverse;

using OpenSim.Services.Interfaces;

namespace AuroraWeb
{
    public partial class Services
    {
        public string LoginRequest(AuroraWeb.Environment env, string first, string last, string password)
        {
            if (!m_WebApp.IsInstalled)
            {
                m_log.DebugFormat("[AuroraWeb]: warning: someone is trying to access LoginRequest and Wifi isn't installed!");
                return m_WebApp.ReadFile(env, "index.html");
            }

            m_log.DebugFormat("[AuroraWeb]: LoginRequest {0} {1}", first, last);
            Request request = env.Request;
            string encpass = Util.Md5Hash(password);

            string notification;
            string authtoken = null;
            UserAccount account = m_UserAccountService.GetUserAccount(UUID.Zero, first, last);
            if (account != null)
                authtoken = m_AuthenticationService.Authenticate(account.PrincipalID, "",encpass, 30);
            if (string.IsNullOrEmpty(authtoken))
                notification = _("Login failed.", env);
            else
            {
                // Successful login
                SessionInfo sinfo;
                sinfo.IpAddress = request.IPEndPoint.Address.ToString();
                sinfo.Sid = authtoken;
                sinfo.Account = account;
                sinfo.Notify = new AuroraWeb.Services.NotificationData();
                m_Sessions.Add(authtoken, sinfo, m_WebApp.SessionTimeout);
                env.Request.Query["sid"] = authtoken;
                env.Session = sinfo;

                List<object> loo = new List<object>();
                loo.Add(account);
                env.Data = loo;
                env.Flags = Flags.IsLoggedIn;
                notification = string.Format(_("Welcome to {0}!", env), m_WebApp.GridName);
            }
            AuroraWeb.Services.NotifyWithoutButton(env, notification);
            return PadURLs(env, authtoken, m_WebApp.ReadFile(env, "index.html"));
        }

        public string LogoutRequest(Environment env)
        {
            m_log.DebugFormat("[AuroraWeb]: LogoutRequest");
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo))
            {
                m_Sessions.Remove(sinfo.Sid);
                m_AuthenticationService.Release(sinfo.Account.PrincipalID, "", sinfo.Sid);
            }

            env.State = State.Default;
            return m_WebApp.ReadFile(env, "index.html");
        }

    }
}
