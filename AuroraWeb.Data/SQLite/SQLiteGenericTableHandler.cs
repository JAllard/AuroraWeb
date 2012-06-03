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
using System.Data;
using System.Reflection;
using Aurora.Framework;
using OpenMetaverse;
using Aurora.DataManager.SQLite;
using Mono.Data.Sqlite;

namespace AuroraWeb.Data.SQLite
{
    public class SQLiteGenericTableHandler<T> where T : class, new()
    {
        protected Assembly Assembly
        {
            get { return GetType().BaseType.Assembly; }
        }
        protected string m_Realm;
        protected static SqliteConnection m_Connection;
        protected Dictionary<string, FieldInfo> m_Fields =
                new Dictionary<string, FieldInfo>();
        protected FieldInfo m_DataField = null;
        protected List<string> m_ColumnNames = null;

        public SQLiteGenericTableHandler(string connectionString, string realm, string storeName)
            : base() { }

        public virtual GridUserData[] GetCount(string field, string key)
        {
            return GetCount(new string[] { field }, new string[] { key });
        }

        public virtual GridUserData[] GetCount(string[] fields, string[] keys)
        {
            if (fields.Length != keys.Length)
                return new GridUserData[] {};

            List<string> terms = new List<string>();

            SqliteCommand cmd = new SqliteCommand();

            for (int i = 0 ; i < fields.Length ; i++)
            {
                cmd.Parameters.Add(new SqliteParameter(":" + fields[i], keys[i]));
                terms.Add("`" + fields[i] + "` = :" + fields[i]);
            }

            string where = String.Join(" and ", terms.ToArray());

            string query = String.Format("select count(*) from {0} where {1}",
                                            m_Realm, where);

            cmd.CommandText = query;

            Object result = DoQueryScalar(cmd);

            return new GridUserData[] {};
        }

        public virtual RegionData[] GetCount(string @where)
        {
            SqliteCommand cmd = new SqliteCommand();

            string query = String.Format("select count(*) from {0} where {1}",
                                             m_Realm, where);

            cmd.CommandText = query;

            Object result = DoQueryScalar(cmd);

            return new RegionData[] {};
        }

        protected object DoQueryScalar(SqliteCommand cmd)
        {
            lock (m_Connection)
            {
                cmd.Connection = m_Connection;
                return cmd.ExecuteScalar();
            }
        }

        protected IDataReader ExecuteReader(SqliteCommand cmd, SqliteConnection connection)
        {
            lock (connection)
            {
                //SqliteConnection newConnection =
                //        (SqliteConnection)((ICloneable)connection).Clone();
                //newConnection.Open();

                //cmd.Connection = newConnection;
                cmd.Connection = connection;
                //Console.WriteLine("XXX " + cmd.CommandText);

                return cmd.ExecuteReader();
            }
        }

        private void CheckColumnNames(IDataReader reader)
        {
            if (m_ColumnNames != null)
                return;

            m_ColumnNames = new List<string>();

            DataTable schemaTable = reader.GetSchemaTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                if (row["ColumnName"] != null &&
                        (!m_Fields.ContainsKey(row["ColumnName"].ToString())))
                    m_ColumnNames.Add(row["ColumnName"].ToString());
            }
        }

        protected internal T[] DoQuery(SqliteCommand cmd)
        {
            IDataReader reader = ExecuteReader(cmd, m_Connection);
            if (reader == null)
                return new T[0];

            CheckColumnNames(reader);

            List<T> result = new List<T>();

            while (reader.Read())
            {
                T row = new T();

                foreach (string name in m_Fields.Keys)
                {
                    if (m_Fields[name].GetValue(row) is bool)
                    {
                        int v = Convert.ToInt32(reader[name]);
                        m_Fields[name].SetValue(row, v != 0 ? true : false);
                    }
                    else if (m_Fields[name].GetValue(row) is UUID)
                    {
                        UUID uuid = UUID.Zero;

                        UUID.TryParse(reader[name].ToString(), out uuid);
                        m_Fields[name].SetValue(row, uuid);
                    }
                    else if (m_Fields[name].GetValue(row) is int)
                    {
                        int v = Convert.ToInt32(reader[name]);
                        m_Fields[name].SetValue(row, v);
                    }
                    else
                    {
                        m_Fields[name].SetValue(row, reader[name]);
                    }
                }
                
                if (m_DataField != null)
                {
                    Dictionary<string, string> data =
                            new Dictionary<string, string>();

                    foreach (string col in m_ColumnNames)
                    {
                        data[col] = reader[col].ToString();
                        if (data[col] == null)
                            data[col] = String.Empty;
                    }

                    m_DataField.SetValue(row, data);
                }

                result.Add(row);
            }

            //CloseCommand(cmd);

            return result.ToArray();
        }
    public interface IGenericData<T>
    {
    }
    public T[] Get(string field, string key)
    {
        return Get(new string[] { field }.ToString(), new string[] { key }.ToString());
    }
        
    }
}

