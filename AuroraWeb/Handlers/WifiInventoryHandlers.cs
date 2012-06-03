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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aurora.Framework.Servers.HttpServer;
using Aurora.Simulation.Base;
using log4net;

using AuroraWeb.Utils;

namespace AuroraWeb
{
    public class WifiInventoryLoadGetHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiInventoryLoadGetHandler(WebApp webapp) :
            base("GET", "/wifi/user/loadinventory")
        {
            m_WebApp = webapp;
        }

        public byte[] Handle(string path, Stream requestData,
                RequestFactory.IOSHttpRequest httpRequest, WifiConsoleHandler.IOSHttpResponse httpResponse)
        {
            Request request = RequestFactory.CreateRequest(string.Empty, httpRequest);
            AuroraWeb.Environment env = new Environment(request);

            string resource = GetParam(path);
            //m_log.DebugFormat("[XXX]: resource {0}", resource);

            string result = m_WebApp.Services.InventoryLoadGetRequest(env);
            httpResponse.ContentType = "text/html";

            return WebAppUtils.StringToBytes(result);
        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class WifiInventoryGetHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiInventoryGetHandler(WebApp webapp) :
                base("GET", "/wifi/user/inventory")
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
            httpResponse.ContentType = "text/html";
            string resource = GetParam(path);
            //m_log.DebugFormat("[INVENTORY HANDLER HANDLER GET]: resource {0}", resource);

            Request request = RequestFactory.CreateRequest(string.Empty, httpRequest);
            AuroraWeb.Environment env = new Environment(request);

            string result = m_WebApp.Services.InventoryGetRequest(env);

            return WebAppUtils.StringToBytes(result);

        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new System.NotImplementedException();
        }
    }

    public class WifiInventoryPostHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiInventoryPostHandler(WebApp webapp) :
            base("POST", "/wifi/user/inventory")
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
            httpResponse.ContentType = "text/html";
            string resource = GetParam(path);
            //m_log.DebugFormat("[INVENTORY HANDLER POST]: resource {0}", resource);

            StreamReader sr = new StreamReader(requestData);
            string body = sr.ReadToEnd();
            sr.Close();
            body = body.Trim();
            Dictionary<string, object> postdata =
                    WebUtils.ParseQueryString(body);

            Request request = RequestFactory.CreateRequest(string.Empty, httpRequest);
            AuroraWeb.Environment env = new Environment(request);

            string action = postdata.Keys.FirstOrDefault(key => key.StartsWith("action-"));
            if (action == null)
                action = string.Empty;
            else
                action = action.Substring("action-".Length);
            
            string folder = string.Empty;
            string newFolderName = string.Empty;
            List<string> nodes = new List<string>();
            List<string> types = new List<string>();

            if (postdata.ContainsKey("folder"))
                folder = postdata["folder"].ToString();
            if (postdata.ContainsKey("newFolderName"))
                newFolderName = postdata["newFolderName"].ToString();
            foreach (KeyValuePair<string, object> kvp in postdata)
            {
                if (kvp.Key.StartsWith("inv-"))
                {
                    nodes.Add(kvp.Key.Substring(4));
                    types.Add(kvp.Value.ToString());
                }
            }

            return WebAppUtils.StringToBytes(m_WebApp.Services.InventoryPostRequest(env, action, folder, newFolderName, nodes, types));

        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new System.NotImplementedException();
        }
    }

}
