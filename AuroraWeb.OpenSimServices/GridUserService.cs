/*
 * Copyright (c) Crista Lopes (aka Diva), JAllard, Enrico Nirvana. All rights reserved.
 * Contributors, http://aurora-sim.org/, http://opensimulator.org/
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
using System.Reflection;
using Aurora.Framework;
using AuroraWeb.Data;
using AuroraWeb.LoginService;
using log4net;
using Nini.Config;

using OpenMetaverse;
using OpenSim.Services.Interfaces;

namespace AuroraWeb.OpenSimServices
{
    public class GridUserService : LLLoginService.IGridUserService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string m_CastWarning = "[WebData]: Invalid cast for GridUser store. AuroraWeb.Data required for method {0}.";
        protected IGridUserData m_Database = null;
        public GridUserService(IConfigSource config)
            : base()
        {
        }

        public List<LLLoginService.GridUserInfo> GetOnlineUsers()
        {
            List<LLLoginService.GridUserInfo> onlineList = new List<LLLoginService.GridUserInfo>();
            try
            {
                GridUserData[] onlines = ((IGridUserData)m_Database).GetOnlineUsers();
                foreach (GridUserData d in onlines)
                    onlineList.Add(ToGridUserInfo(d));

            }
            catch (InvalidCastException)
            {
                m_log.WarnFormat(m_CastWarning, MethodBase.GetCurrentMethod().Name);
            }

            return onlineList;
        }

        public int GetOnlineUserCount()
        {
            try
            {
                return Convert.ToInt32((m_Database).GetOnlineUserCount().ToString());
            }
            catch (InvalidCastException)
            {
                m_log.WarnFormat(m_CastWarning, MethodBase.GetCurrentMethod().Name);
            }

            return 0;
        }

        public long GetActiveUserCount(int period)
        {
            try
            {
                return Convert.ToInt64(((IGridUserData)m_Database).GetActiveUserCount(period).ToString());
            }
            catch (InvalidCastException)
            {
                m_log.WarnFormat(m_CastWarning, MethodBase.GetCurrentMethod().Name);
            }

            return 0;
        }
    
        protected LLLoginService.GridUserInfo ToGridUserInfo(GridUserData d)
        {
            LLLoginService.GridUserInfo info = new LLLoginService.GridUserInfo();
            info.UserID = d.UserID;
            info.HomeRegionID = new UUID(d.Data["HomeRegionID"]);
            info.HomePosition = Vector3.Parse(d.Data["HomePosition"]);
            info.HomeLookAt = Vector3.Parse(d.Data["HomeLookAt"]);

            info.LastRegionID = new UUID(d.Data["LastRegionID"]);
            info.LastPosition = Vector3.Parse(d.Data["LastPosition"]);
            info.LastLookAt = Vector3.Parse(d.Data["LastLookAt"]);

            info.Online = bool.Parse(d.Data["Online"]);
            info.Login = Util.ToDateTime(Convert.ToInt32(d.Data["Login"]));
            info.Logout = Util.ToDateTime(Convert.ToInt32(d.Data["Logout"]));

            return info;
        }

        public LLLoginService.GridUserInfo LoggedIn(string userID)
        {
            throw new NotImplementedException();
        }

        public bool LoggedOut(string userID, UUID sessionID, UUID regionID, Vector3 lastPosition, Vector3 lastLookAt)
        {
            throw new NotImplementedException();
        }

        public bool SetHome(string userID, UUID homeID, Vector3 homePosition, Vector3 homeLookAt)
        {
            throw new NotImplementedException();
        }

        public bool SetLastPosition(string userID, UUID sessionID, UUID regionID, Vector3 lastPosition, Vector3 lastLookAt)
        {
            throw new NotImplementedException();
        }

        public LLLoginService.GridUserInfo GetGridUserInfo(string userID)
        {
            throw new NotImplementedException();
        }

        public LLLoginService.GridUserInfo[] GetGridUserInfo(string[] userID)
        {
            throw new NotImplementedException();
        }
    }
}
