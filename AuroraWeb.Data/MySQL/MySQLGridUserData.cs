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

using System.Collections.Generic;
using System.Reflection;
using Aurora.Framework;
using AuroraWeb.Data;
using Nini.Config;
using OpenMetaverse;
using OpenSim.Services.Interfaces;

namespace AuroraWeb.Data.MySQL
{
    public class MySQLGridUserData<T> :  IGridUserData
    {
        private MySQLGenericTableHandler<GridUserData> m_DatabaseHandler;

        protected Assembly Assembly
        {
            get { return GetType().BaseType.Assembly; }
        }

        public MySQLGridUserData(string connectionString, string realm)
            
        {
            m_DatabaseHandler = new MySQLGenericTableHandler<GridUserData>(connectionString, realm, "GridUserStore");
        }

        public GridUserData[] GetOnlineUsers()
        {
            return m_DatabaseHandler.GetCount("Online", true.ToString());
        }
        
        public virtual T[] Get(string field, string key)
        {
            return Get(new string[] { field }.ToString(), new string[] { key }.ToString());
        }
        
        public GridUserData[] GetOnlineUserCount()
        {
            return m_DatabaseHandler.GetCount("Online", true.ToString());
        }

        public RegionData[] GetActiveUserCount(int period)
        {
            return m_DatabaseHandler.GetCount(string.Format("Online = '{0}' OR DATEDIFF(CURRENT_DATE(), FROM_UNIXTIME(Logout)) <= {1}", true, period));
        }

        public IAgentInfoService InnerService
        {
            get { throw new System.NotImplementedException(); }
        }

        public UserInfo GetUserInfo(string userID)
        {
            throw new System.NotImplementedException();
        }

        public List<UserInfo> GetUserInfos(List<string> userIDs)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetAgentsLocations(string requestor, List<string> userIDs)
        {
            throw new System.NotImplementedException();
        }

        public bool SetHomePosition(string userID, UUID homeID, Vector3 homePosition, Vector3 homeLookAt)
        {
            throw new System.NotImplementedException();
        }

        public void SetLastPosition(string userID, UUID regionID, Vector3 lastPosition, Vector3 lastLookAt)
        {
            throw new System.NotImplementedException();
        }

        public void SetLoggedIn(string userID, bool loggingIn, bool fireLoggedInEvent, UUID enteringRegion)
        {
            throw new System.NotImplementedException();
        }

        public void LockLoggedInStatus(string userID, bool locked)
        {
            throw new System.NotImplementedException();
        }

        public void Start(IConfigSource config, IRegistryCore registry)
        {
            throw new System.NotImplementedException();
        }

        public void FinishedStartup()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IConfigSource config, IRegistryCore registry)
        {
            throw new System.NotImplementedException();
        }
    }
}
