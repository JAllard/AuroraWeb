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
using System;
using System.Collections;
using System.Collections.Generic;
using Aurora.Framework;
using OpenMetaverse;
using Nwc.XmlRpc;
using GridRegion = OpenSim.Services.Interfaces.GridRegion;

namespace AuroraWeb
{
    public partial class Services
    {
        public string RegionManagementShutdownPostRequest(AuroraWeb.Environment env)
        {
            //m_log.DebugFormat("[AuroraWeb]: RegionManagementShutdownPostRequest");
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;

                //FIXME: don't hardcode url, get it from m_GridService
                //TODO: check if server is actually running first
                //TODO: add support for shutdown message parameter from html form
                string url = m_WebApp.LoginURL;
                Hashtable hash = new Hashtable();
                if (m_ServerAdminPassword == null)
                {
                    m_log.Debug("[RegionManagementShutdownPostRequest] No remote admin password was set in .ini file");
                }

                hash["password"] = m_ServerAdminPassword;
                IList paramList = new ArrayList();
                paramList.Add(hash);
                XmlRpcRequest xmlrpcReq = new XmlRpcRequest("admin_shutdown", paramList);

                XmlRpcResponse response = null;
                try
                {
                    response = xmlrpcReq.Send(url, 10000);
                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    env.State = State.RegionManagementSuccessful;
                }
                catch (Exception e)
                {
                    m_log.Debug("[AuroraWeb]: Exception " + e.Message);
                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    env.State = State.RegionManagementUnsuccessful;
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");
        }

        public string RegionManagementRestartPostRequest(AuroraWeb.Environment env)
        {
            Request request = env.Request;
            SessionInfo sinfo;

            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;

                string url = m_WebApp.LoginURL;

                Hashtable hash = new Hashtable();
                if (m_ServerAdminPassword == null)
                {
                    m_log.Debug("[RegionManagementRestartPostRequest] No remote admin password was set in .ini file");
                }

                hash["password"] = m_ServerAdminPassword;
                IList paramList = new ArrayList();
                paramList.Add(hash);
                XmlRpcRequest xmlrpcReq = new XmlRpcRequest("admin_shutdown", paramList);

                XmlRpcResponse response = null;
                try
                {
                    //first, shutdown the server
                    response = xmlrpcReq.Send(url, 10000);

                    //then wait until the server is completely shutdown, then re-launch
                    System.Diagnostics.Process[] openSimProcess = System.Diagnostics.Process.GetProcessesByName("OpenSim");
                    openSimProcess[0].WaitForExit();
                    System.Diagnostics.Process.Start("OpenSim.exe");
                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    env.State = State.RegionManagementSuccessful;
                }
                catch (Exception e)
                {
                    m_log.Debug("[AuroraWeb]: Exception " + e.Message);
                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    env.State = State.RegionManagementUnsuccessful;
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");

        }

        public string RegionManagementBroadcastPostRequest(AuroraWeb.Environment env, string message)
        {
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;

                string url = m_WebApp.LoginURL;
                Hashtable hash = new Hashtable();
                if (m_ServerAdminPassword == null)
                {
                    m_log.Debug("[RegionManagementBroadcastPostRequest] No remote admin password was set in .ini file");
                }

                hash["password"] = m_ServerAdminPassword;
                hash["message"] = message;
                IList paramList = new ArrayList();
                paramList.Add(hash);
                XmlRpcRequest xmlrpcReq = new XmlRpcRequest("admin_broadcast", paramList);

                XmlRpcResponse response = null;
                try
                {
                    response = xmlrpcReq.Send(url, 10000);
                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    env.State = State.RegionManagementSuccessful;
                }
                catch (Exception e)
                {
                    m_log.Debug("[AuroraWeb]: Exception " + e.Message);
                    env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                    env.State = State.RegionManagementUnsuccessful;
                }

                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");
        }

        public string RegionManagementGetRequest(AuroraWeb.Environment env)
        {
            m_log.DebugFormat("[Services]: RegionManagementGetRequest()");
            Request request = env.Request;

            SessionInfo sinfo;
            if (TryGetSessionInfo(request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                var regions = m_GridService.GetRegionsByName(UUID.Zero, "", 200);

                MainConsole.Instance.DebugFormat("[Services]: There are {0} regions", regions.Count);
                //regions.ForEach(delegate(GridRegion region);
                //(
                  //  m_log.DebugFormat("[Services] {0}", gg.RegionName);
                //);

                env.Session = sinfo;
                //env.Data = Objectify(regions);
                env.Flags = Flags.IsAdmin | Flags.IsLoggedIn;
                env.State = State.RegionManagementForm;
                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }
            else
            {
                return m_WebApp.ReadFile(env, "index.html");
            }
        }

        private List<object> GetRegionList(Environment env)
        {
            var regions = m_GridService.GetRegionsByName(UUID.Zero, "", 100);

            if (regions != null)
            {
                m_log.DebugFormat("[AuroraWeb]: GetRegionList found {0} users in DB", regions.Count);
                return Objectify<GridRegion>(null);
            }
            else
            {
                m_log.DebugFormat("[AuroraWeb]: GetRegionList got null regions from DB");
                return new List<object>();
            }

        }

    }
}
