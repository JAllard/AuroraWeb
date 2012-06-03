/**
 * Copyright (c) Marck, JAllard, Enrico Nirvana. All rights reserved.
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
using System.Reflection;
using Aurora.Framework;
using log4net;
using Nini.Config;
using OpenMetaverse;

using OpenSim.Services.Interfaces;
using GridRegion = OpenSim.Services.Interfaces.GridRegion;
using RegionFlags = Aurora.Framework.RegionFlags;

namespace Aurora.OpenSimServices
{
    public class GridService : IGridService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string m_CastWarning = "[WebData]: Invalid cast for Grid store. AuroraWeb.Data required for method {0}.";
        protected IRegionData m_Database = null;
        public GridService(IConfigSource config) : base() 
        {
        }

        public GridRegion TryLinkRegionToCoords(UUID scopeID, string address, uint xloc, uint yloc, UUID ownerID, out string reason)
        {
            //return m_HypergridLinker.TryLinkRegionToCoords(scopeID, address, (int)xloc, (int)yloc, ownerID, out reason);
            reason = null;
            return null;
        }

        public bool TryUnlinkRegion(string mapName)
        {
            //return m_HypergridLinker.TryUnlinkRegion(mapName);
            return false;
        }

        public long GetLocalRegionsCount(UUID scopeID)
        {
            try
            {
                const RegionFlags flags = RegionFlags.RegionOnline;
                const RegionFlags excludeFlags = RegionFlags.Hyperlink;
                return ((IRegionData)m_Database).Count(flags,excludeFlags);
            }
            catch (InvalidCastException)
            {
                m_log.WarnFormat(m_CastWarning, MethodBase.GetCurrentMethod().Name);
            }
            return 0;
        }

        public IGridService InnerService
        {
            get { throw new NotImplementedException(); }
        }

        public int GetMaxRegionSize()
        {
            throw new NotImplementedException();
        }

        public int GetRegionViewSize()
        {
            throw new NotImplementedException();
        }

        public RegisterRegion RegisterRegion(GridRegion regionInfos, UUID oldSessionID)
        {
            throw new NotImplementedException();
        }

        public bool DeregisterRegion(GridRegion region)
        {
            throw new NotImplementedException();
        }

        public GridRegion GetRegionByUUID(UUID scopeID, UUID regionID)
        {
            throw new NotImplementedException();
        }

        public GridRegion GetRegionByPosition(UUID scopeID, int x, int y)
        {
            throw new NotImplementedException();
        }

        public GridRegion GetRegionByName(UUID scopeID, string regionName)
        {
            throw new NotImplementedException();
        }

        public List<GridRegion> GetRegionsByName(UUID scopeID, string name, int maxNumber)
        {
            throw new NotImplementedException();
        }

        public List<GridRegion> GetRegionRange(UUID scopeID, int xmin, int xmax, int ymin, int ymax)
        {
            throw new NotImplementedException();
        }

        public List<GridRegion> GetRegionRange(UUID scopeID, float centerX, float centerY, uint squareRangeFromCenterInMeters)
        {
            throw new NotImplementedException();
        }

        public List<GridRegion> GetNeighbors(GridRegion region)
        {
            throw new NotImplementedException();
        }

        public List<GridRegion> GetDefaultRegions(UUID scopeID)
        {
            throw new NotImplementedException();
        }

        public List<GridRegion> GetFallbackRegions(UUID scopeID, int x, int y)
        {
            throw new NotImplementedException();
        }

        public List<GridRegion> GetSafeRegions(UUID scopeID, int x, int y)
        {
            throw new NotImplementedException();
        }

        public int GetRegionFlags(UUID scopeID, UUID regionID)
        {
            throw new NotImplementedException();
        }

        public string UpdateMap(GridRegion region)
        {
            throw new NotImplementedException();
        }

        public multipleMapItemReply GetMapItems(ulong regionHandle, GridItemType gridItemType)
        {
            throw new NotImplementedException();
        }

        public void SetRegionUnsafe(UUID RegionID)
        {
            throw new NotImplementedException();
        }

        public void SetRegionSafe(UUID RegionID)
        {
            throw new NotImplementedException();
        }

        public bool VerifyRegionSessionID(GridRegion r, UUID SessionID)
        {
            throw new NotImplementedException();
        }

        public void Configure(IConfigSource config, IRegistryCore registry)
        {
            throw new NotImplementedException();
        }

        public void Start(IConfigSource config, IRegistryCore registry)
        {
            throw new NotImplementedException();
        }

        public void FinishedStartup()
        {
            throw new NotImplementedException();
        }
    }
}
