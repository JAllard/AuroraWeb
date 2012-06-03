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
using Nini.Config;
using OpenMetaverse;
using OpenSim.Services.Interfaces;

namespace AuroraWeb.Data.SQLite
{
    /// <summary>
    /// A RegionData Interface to the SQLite database
    /// </summary>
    public class SQLiteRegionData : Aurora.Framework.IRegionData
    {
        protected Assembly Assembly
        {
            get { return GetType().BaseType.Assembly; }
        }

        private SQLiteGenericTableHandler<RegionData> m_DatabaseHandler;

        public SQLiteRegionData(string connectionString, string realm)
            
        {
            m_DatabaseHandler = new SQLiteGenericTableHandler<RegionData>(connectionString, realm, "GridStore");
        }

        public RegionData[] Get(UUID scopeID, int regionFlags, int excludeFlags)
        {
            return m_DatabaseHandler.GetCount(CreateWhereClause(scopeID, regionFlags, excludeFlags));
        }

        public RegionData[] GetCount(UUID scopeID, int regionFlags, int excludeFlags)
        {
            return m_DatabaseHandler.GetCount(CreateWhereClause(scopeID, regionFlags, excludeFlags));
        }

        private string CreateWhereClause(UUID scopeID, int regionFlags, int excludeFlags)
        {
            string where = "(flags & {0}) <> 0 and (flags & {1}) = 0";
            if (scopeID != UUID.Zero)
                where += " and ScopeID = " + scopeID.ToString();
            return string.Format(where, regionFlags.ToString(), excludeFlags.ToString());
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public void Initialize(IGenericData GenericData, IConfigSource source, IRegistryCore simBase, string DefaultConnectionString)
        {
            throw new System.NotImplementedException();
        }

        public GridRegion Get(UUID regionID, UUID ScopeID)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(string regionName, UUID ScopeID)
        {
            throw new System.NotImplementedException();
        }

        public GridRegion GetZero(int x, int y, UUID ScopeID)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(int x, int y, UUID ScopeID)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(RegionFlags regionFlags)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(int xStart, int yStart, int xEnd, int yEnd, UUID ScopeID)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(RegionFlags flags, Dictionary<string, bool> sort)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(uint start, uint count, uint EstateID, RegionFlags flags, Dictionary<string, bool> sort)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(RegionFlags includeFlags, RegionFlags excludeFlags, uint? start, uint? count, Dictionary<string, bool> sort)
        {
            throw new System.NotImplementedException();
        }

        public uint Count(RegionFlags includeFlags, RegionFlags excludeFlags)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> GetNeighbours(UUID regionID, UUID ScopeID, uint squareRangeFromCenterInMeters)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> Get(UUID scopeID, UUID excludeRegion, float centerX, float centerY, uint squareRangeFromCenterInMeters)
        {
            throw new System.NotImplementedException();
        }

        public uint Count(uint EstateID, RegionFlags flags)
        {
            throw new System.NotImplementedException();
        }

        public bool Store(GridRegion data)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(UUID regionID)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteAll(string[] criteriaKey, object[] criteriaValue)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> GetDefaultRegions(UUID scopeID)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> GetFallbackRegions(UUID scopeID, int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public List<GridRegion> GetSafeRegions(UUID scopeID, int x, int y)
        {
            throw new System.NotImplementedException();
        }
    }
}
