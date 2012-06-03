/**
 * Copyright (c) Crista Lopes (aka Diva), JAllard, Enrico Nirvana. All rights reserved.
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
using System.Reflection;
using System.Runtime.CompilerServices;
using Aurora.Framework;
using Aurora.Simulation.Base;
using OpenSim.Region.Framework;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using OpenMetaverse;
using OpenMetaverse.Imaging;
using log4net;
using Nini.Config;


namespace Aurora.Web
{
    public class WifiModule : ISharedRegionModule
    {
        #region Class and Instance Members

        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool m_enabled = false;
        
        #endregion

        #region ISharedRegionModule

        public void Initialise(IConfigSource config)
        {
            m_log.Info("[Web Module] Initializing...");

            try
            {
                m_enabled = config.Configs["Modules"].GetBoolean("WebModule", m_enabled);
                if (m_enabled)
                {
                    object[] args = new object[] { config, MainServer.Instance, string.Empty };
                    //new WifiServerConnector(config, MainServer.Instance, string.Empty);

                    AuroraModuleLoader.LoadPlugins<IService>("Aurora.Web.dll:WifiServerConnector");
                    m_log.Debug("[Web Module]: AuroraWeb enabled.");
                }

            }
            catch (Exception e)
            {
                m_log.ErrorFormat(e.StackTrace);
                m_log.ErrorFormat("[Web Module]: Could not load AuroraWeb: {0}. ", e.Message);
                m_enabled = false;
                return;
            }

        }

        public void AddRegion(IScene scene)
        {
            throw new NotImplementedException();
        }

        public void RegionLoaded(IScene scene)
        {
            throw new NotImplementedException();
        }

        public void RemoveRegion(IScene scene)
        {
            throw new NotImplementedException();
        }

        public bool IsSharedModule
        {
            get { return true; }
        }

        public string Name
        {
            get { return "Web Service"; }
        }

        public Type ReplaceableInterface 
        { 
            get { return null; }
        }

        public void AddRegion(Scene scene)
        {
        }

        public void RemoveRegion(Scene scene)
        {
        }

        public void RegionLoaded(Scene scene)
        {
        }

        public void PostInitialise()
        {
        }

        public void Close()
        {
        }


        #endregion

    }
}
