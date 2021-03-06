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

using Aurora.Framework.Servers.HttpServer;
using AuroraWeb;
using Nini.Config;
using log4net;
using System;
using System.Reflection;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Web;

using System.Collections.Generic;
using Aurora.Server;
using OpenSim.Services.Interfaces;
using Aurora.Framework;

using OpenMetaverse;

using AuroraWeb.WifiScript;
using AuroraWeb.Utils;

using Processor = AuroraWeb.WifiScript.Processor;

namespace AuroraWeb
{
    public class WifiDefaultHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiDefaultHandler(WebApp webapp) :
                base("GET", "/wifi")
        {
            m_WebApp = webapp;
        }

        public byte[] Handle(string path, Stream requestData,
                RequestFactory.IOSHttpRequest httpRequest, WifiConsoleHandler.IOSHttpResponse httpResponse)
        {
            // path = /wifi/...
            //m_log.DebugFormat("[AuroraWeb]: path = {0}", path);
            //m_log.DebugFormat("[AuroraWeb]: ip address = {0}", httpRequest.RemoteIPEndPoint);
            //foreach (object o in httpRequest.Query.Keys)
            //    m_log.DebugFormat("  >> {0}={1}", o, httpRequest.Query[o]);

            string resource = GetParam(path);
            //m_log.DebugFormat("[AuroraWeb]: resource {0}", resource);
            resource = Uri.UnescapeDataString(resource).Trim(WebAppUtils.DirectorySeparatorChars);

            Request request = RequestFactory.CreateRequest(resource, httpRequest);
            AuroraWeb.Environment env = new AuroraWeb.Environment(request);

            if (resource == string.Empty || resource.StartsWith("index."))
            {
                if (m_WebApp.StatisticsUpdateInterval != TimeSpan.Zero)
                    m_WebApp.Services.ComputeStatistics();

                httpResponse.ContentType = "text/html";

                return WebAppUtils.StringToBytes(m_WebApp.Services.DefaultRequest(env));
            }
            else
            {
                string resourcePath = System.IO.Path.Combine(WebApp.DocsPath, resource);
                string type = WebAppUtils.GetContentType(resource);
                httpResponse.ContentType = type;
                //m_log.DebugFormat("[AuroraWeb]: ContentType {0}", type);
                if (type.StartsWith("image"))
                    return WebAppUtils.ReadBinaryResource(resourcePath);

                if (type.StartsWith("application"))
                {
                    string res = WebAppUtils.ReadTextResource(resourcePath, true);
                    return WebAppUtils.StringToBytes(res);
                }
                if (type.StartsWith("text"))
                {
                    if (m_WebApp.StatisticsUpdateInterval != TimeSpan.Zero)
                        m_WebApp.Services.ComputeStatistics();

                    resourcePath = Localization.LocalizePath(env, resource);
                    Processor p = new Processor(m_WebApp.WifiScriptFace, env);
                    string res = p.Process(WebAppUtils.ReadTextResource(resourcePath));
                    if (res == string.Empty)
                        res = m_WebApp.Services.DefaultRequest(env);
                    return WebAppUtils.StringToBytes(res);
                }
            }

            httpResponse.ContentType = "text/plain";
            string result = "Boo!";
            return WebAppUtils.StringToBytes(result);
        }

        /*
        private string GetResource(string path)
        {
            string[] paramArray = SplitParams(path);
            m_log.DebugFormat("[AuroraWeb]: paramArray length = {0}", paramArray.Length);
            if (paramArray != null && paramArray.Length > 0)
                return paramArray[0];

            return string.Empty;
        }
        */
        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }
    }
}
