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
using System.Reflection;
using Aurora.Framework;
using MySql.Data.MySqlClient;
using Aurora.DataManager.MySQL;

namespace AuroraWeb.Data.MySQL
{
    public class MySQLGenericTableHandler<T> : IGenericData <T> where T : class, new()
    {
        protected Assembly Assembly
        {
            get { return GetType().BaseType.Assembly; }
        }

        public MySQLGenericTableHandler(string connectionString, string realm, string storeName)
             { }

        public virtual GridUserData[] GetCount(string field, string key)
        {
            return GetCount(new string[] { field }, new string[] { key });
        }

        public virtual GridUserData[] GetCount(string[] fields, string[] keys)
        {
            if (fields.Length != keys.Length)
                return new GridUserData[] {};

            List<string> terms = new List<string>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    cmd.Parameters.AddWithValue(fields[i], keys[i]);
                    terms.Add("`" + fields[i] + "` = ?" + fields[i]);
                }

                string where = String.Join(" and ", terms.ToArray());

                object m_Realm = null;
                string query = String.Format("select count(*) from {0} where {1}",
                                             m_Realm, where);

                cmd.CommandText = query;

                Object result = DoQueryScalar(cmd);

                return new GridUserData[] {};
            }
        }

        public virtual RegionData[] GetCount(string @where)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                object m_Realm = null;
                string query = String.Format("select count(*) from {0} where {1}",
                                             m_Realm, where);

                cmd.CommandText = query;

                object result = DoQueryScalar(cmd);

                return new RegionData[] {};
            }
        }

        public void ConnectToDatabase(string connectionstring, string migratorName, bool validateTables)
        {
            string m_connectionString = connectionstring;
            MySqlConnection c = new MySqlConnection(connectionstring);
            
        }
        
        public virtual object DoQueryScalar(MySqlCommand cmd)
        {
            string connectionstring = null;
            string m_connectionString = string.Empty;
            if (m_connectionString == null) throw new ArgumentNullException("");
            ConnectToDatabase(connectionstring, "",true);
            using (MySqlConnection dbcon = new MySqlConnection(m_connectionString))
            {
                dbcon.Open();
                cmd.Connection = dbcon;

                return cmd.ExecuteScalar();
            }
        }

        

        new public virtual T[] DoQuery(MySqlCommand cmd)
        {
            return new T[] {};
        }
    }

    public interface IGenericData<T>
    {
    }
}