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
using System.Reflection;
using Aurora.Framework;
using Nini.Config;
using OpenMetaverse;
using OpenSim.Region.Framework.Interfaces;

namespace AuroraWeb.Data.MySQL
{
    public class MySQLEstateStore : IEstateModule
    {
        protected Assembly Assembly
        {
            get { return GetType().BaseType.Assembly; }
        }

        public MySQLEstateStore()
            : base()
        {
        }

        public MySQLEstateStore(string connectionString)
            
        {
        }

        public string Name
        {
            get { return GetType().AssemblyQualifiedName; }
        }

        public Type ReplaceableInterface
        {
            get { return null; }
        }

        public void Initialise(IConfigSource source)
        {
            
        }

        public void AddRegion(IScene scene)
        {
            scene.RegisterModuleInterface<MySQLEstateStore>(this);
            scene.RegisterModuleInterface(this);
        }

        public void RegionLoaded(IScene scene)
        {
            
        }

        public void RemoveRegion(IScene scene)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public event ChangeDelegate OnRegionInfoChange;
        public event ChangeDelegate OnEstateInfoChange;
        public event MessageDelegate OnEstateMessage;
        public uint GetRegionFlags()
        {
            throw new NotImplementedException();
        }

        public bool IsManager(UUID avatarID)
        {
            throw new NotImplementedException();
        }

        public void sendRegionHandshakeToAll()
        {
            throw new NotImplementedException();
        }

        public void setEstateTerrainBaseTexture(int level, UUID texture)
        {
            throw new NotImplementedException();
        }

        public void setEstateTerrainTextureHeights(int corner, float lowValue, float highValue)
        {
            throw new NotImplementedException();
        }

        public void TriggerEstateSunUpdate()
        {
            throw new NotImplementedException();
        }

        public void SetSceneCoreDebug(bool ScriptEngine, bool CollisionEvents, bool PhysicsEngine)
        {
            throw new NotImplementedException();
        }
    }
}
