﻿/**
 * Copyright (c) Marcus Kirsch (aka Marck), JAllard, Enrico Nirvana. All rights reserved.
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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using Aurora.Framework;
using OpenMetaverse;

using GridRegion = OpenSim.Services.Interfaces.GridRegion;

namespace AuroraWeb
{
    public partial class Services
    {
        public string ConsoleRequest(Environment env)
        {
            MainConsole.Instance.DebugFormat("[AuroraWeb]: ConsoleRequest");
            SessionInfo sinfo;
            if (TryGetSessionInfo(env.Request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                env.Session = sinfo;
                env.Flags = Flags.IsLoggedIn | Flags.IsAdmin;
                env.State = State.Console;
                return PadURLs(env, sinfo.Sid, m_WebApp.ReadFile(env, "index.html"));
            }

            return m_WebApp.ReadFile(env, "index.html");
        }

        public string ConsoleHeartbeat(Environment env)
        {
            MainConsole.Instance.DebugFormat("[AuroraWeb]: ConsoleHeartbeat");
            SessionInfo sinfo;
            if (TryGetSessionInfo(env.Request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                // Reset session timer
                m_Sessions.Update(sinfo.Sid, sinfo, m_WebApp.SessionTimeout);
            }

            return string.Empty;
        }

        public string ConsoleSimulatorsRequest(Environment env)
        {
            MainConsole.Instance.Debug("[AuroraWeb]: ConsoleSimulatorsRequest");
            string result = string.Empty;

            SessionInfo sinfo;
            if (TryGetSessionInfo(env.Request, out sinfo) &&
                (sinfo.Account.UserLevel >= m_WebApp.AdminUserLevel))
            {
                // Retrieve addresses of simulators (and the regions running on them)
                List<GridRegion> allRegions2 = new List<GridRegion>(200);
                List<GridRegion> allRegions = new List<GridRegion>((IEnumerable<GridRegion>) m_GridService.GetRegionsByName(UUID.Zero, "", 200));
                string hyperlinks = AuroraWeb.WifiScriptFace.GetHyperlinks(env);
                IEnumerable<GridRegion> regions = allRegions;

                NameValueCollection simulators = new NameValueCollection();
                if (regions != null)
                {
                    foreach (GridRegion region in regions)
                    {
                        string address = region.ExternalHostName + ':' + region.HttpPort;
                        simulators.Add(address, region.RegionName);
                    }
                }

                // Create an XML document with the result data
                XmlDocument xmldoc = new XmlDocument();
                XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);

                XmlElement rootElement = xmldoc.CreateElement("Wifi");
                xmldoc.AppendChild(rootElement);

                XmlElement simulatorsElement = xmldoc.CreateElement("Simulators");
                rootElement.AppendChild(simulatorsElement);

                foreach (string address in simulators.AllKeys)
                {
                    XmlElement simElement = xmldoc.CreateElement("Simulator");
                    simElement.SetAttribute("HostAddress", address);
                    simulatorsElement.AppendChild(simElement);

                    foreach (string regionName in simulators.GetValues(address))
                    {
                        XmlElement regionElement = xmldoc.CreateElement("Region");
                        simElement.AppendChild(regionElement);

                        XmlElement nameElement = xmldoc.CreateElement("Name");
                        nameElement.AppendChild(xmldoc.CreateTextNode(regionName));
                        regionElement.AppendChild(nameElement);
                    }
                }

                result = xmldoc.InnerXml;
            }
            return result;
        }
    }
}
