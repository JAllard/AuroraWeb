/*
 * Copyright (c) Crista Lopes (aka Diva), Marcus Kirsch (aka Marck), JAllard, Enrico Nirvana (aka Rico). All rights reserved.
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
using System.Linq;
using System.Reflection;
using AuroraWeb.Data.MySQL;
using log4net;
using Nini.Config;

using OpenMetaverse;
using OpenSim.Services.Interfaces;
using AuroraWeb.Data;
using IUserAccountData = AuroraWeb.Data.IUserAccountData;

namespace AuroraWeb.OpenSimServices
{
    public class UserAccountService : OpenSim.Services.UserAccountService.UserAccountService, IUserAccountService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string m_CastWarning = "[WebData]: Invalid cast for UserAccount store. AuroraWeb.Data required for method {0}.";

        public UserAccountService(IConfigSource config)
            : base()
        {
        }

        public bool DeleteAccount(UUID scopeID, UUID userID)
        {
            //return AuroraWeb.Data.IUserAccountData.Delete("PrincipalID", userID.ToString());
            return false;
        }

        /// <summary>
        /// An interface for connecting to the user accounts datastore
        /// </summary>
        public interface IUserAccountData
        {
            MySQLUserAccountData.UserAccountData[] Get(string[] fields, string[] values);
            bool Store(MySQLUserAccountData.UserAccountData data);
            new bool Delete(string principalid, string toString);
            MySQLUserAccountData.UserAccountData[] GetUsers(UUID scopeID, string query);
        }
        public List<UserAccount> GetActiveAccounts(UUID scopeID, string term, string excludeTerm)
        {
            List<UserAccount> activeAccounts = new List<UserAccount>();
            try
            {
                UserAccount[] accounts = ((AuroraWeb.Data.IUserAccountData)m_Database).GetActiveAccounts(scopeID, term, excludeTerm);
                foreach (UserAccount d in accounts)
                {
                    activeAccounts.Add(ToUserAccount(null));
                }
            }
            catch (InvalidCastException)
            {
                m_log.WarnFormat(m_CastWarning, MethodBase.GetCurrentMethod().Name);
            }

            return activeAccounts;
        }

        public long GetActiveAccountsCount(UUID scopeID, string excludeTerm)
        {
            try
            {
                return Convert.ToInt64(((AuroraWeb.Data.IUserAccountData)m_Database).GetActiveAccountsCount(scopeID, excludeTerm).ToString());
            }
            catch (InvalidCastException)
            {
                m_log.WarnFormat(m_CastWarning, MethodBase.GetCurrentMethod().Name);
            }

            return 0;
        }

        protected UserAccount ToUserAccount(MySQLUserAccountData.UserAccountData d)
        {
            UserAccount account = new UserAccount(new UUID(d.Data.ToDictionary(p => p.Key, p => (object)p.Value).ToString()));
            account.PrincipalID = d.PrincipalID;
            account.ScopeID = d.ScopeID;
            //account.FirstName = d.FirstName;
            //account.LastName = d.LastName;
            return account;
        }
    }
}
