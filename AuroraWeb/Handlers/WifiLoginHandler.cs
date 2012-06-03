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
using Aurora.Simulation.Base;
using log4net;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using AuroraWeb.Utils;

namespace AuroraWeb
{
    public class WifiLoginHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiLoginHandler(WebApp webapp) :
                base("POST", "/wifi/login")
        {
            m_WebApp = webapp;
        }

        public byte[] Handle(string path, Stream requestData,
                RequestFactory.IOSHttpRequest httpRequest, WifiConsoleHandler.IOSHttpResponse httpResponse)
        {
            StreamReader sr = new StreamReader(requestData);
            string body = sr.ReadToEnd();
            sr.Close();
            body = body.Trim();

            httpResponse.ContentType = "text/html";

            string resource = GetParam(path);

            string method = string.Empty;
            try
            {
                Dictionary<string, object> request =
                        WebUtils.ParseQueryString(body);

                if (!request.ContainsKey("METHOD"))
                    return WebAppUtils.FailureResult();

                method = request["METHOD"].ToString();

                switch (method)
                {
                    case "login":
                        return LoginAgent(resource, httpRequest, request);
                }
                m_log.DebugFormat("[PRESENCE HANDLER]: unknown method request: {0}", method);
            }
            catch (Exception e)
            {
                m_log.DebugFormat("[PRESENCE HANDLER]: Exception in method {0}: {1}", method, e);
            }

            return WebAppUtils.FailureResult();

        }

        byte[] LoginAgent(string resource, RequestFactory.IOSHttpRequest httpRequest, Dictionary<string, object> request)
        {
            string first = String.Empty;
            string last = String.Empty;
            string password = String.Empty;

            if (!request.ContainsKey("firstname") || !request.ContainsKey("lastname") || !request.ContainsKey("password"))
                return WebAppUtils.FailureResult();

            first = request["firstname"].ToString();
            last = request["lastname"].ToString();
            password = request["password"].ToString();

            Request req = RequestFactory.CreateRequest(resource, httpRequest);
            AuroraWeb.Environment env = new Environment(req);

            string result = m_WebApp.Services.LoginRequest(env, first, last, password);

            return WebAppUtils.StringToBytes(result);
        }


        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }
    }
}
