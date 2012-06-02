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
using OpenMetaverse;

using Aurora.Framework;

namespace AuroraWeb.Data
{
    public interface IRegionData : Aurora.Framework.IRegionData
    {
        RegionData[] Get(UUID scopeID, int regionFlags, int excludeFlags);
        long GetCount(UUID scopeID, int regionFlags, int excludeFlags);
    }
    public class RegionData
    {
        public UUID RegionID;
        public UUID ScopeID;
        public string RegionName;

        /// <summary>
        /// The position in meters of this region.
        /// </summary>
        public int posX;

        /// <summary>
        /// The position in meters of this region.
        /// </summary>
        public int posY;

        public int sizeX;
        public int sizeY;

        /// <summary>
        /// Return the x-coordinate of this region.
        /// </summary>
        public int coordX { get { return posX / (int)Constants.RegionSize; } }

        /// <summary>
        /// Return the y-coordinate of this region.
        /// </summary>
        public int coordY { get { return posY / (int)Constants.RegionSize; } }

        public Dictionary<string, object> Data;
    }
}
