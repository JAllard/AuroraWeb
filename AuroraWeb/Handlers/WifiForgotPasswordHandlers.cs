﻿/**
* Copyright (c) Crista Lopes (aka Diva), Sam Kaufman. All rights reserved.
*
* Redistribution and use in source and binary forms, with or without modification,
* are permitted provided that the following conditions are met:
*
* * Redistributions of source code must retain the above copyright notice,
* this list of conditions and the following disclaimer.
* * Redistributions in binary form must reproduce the above copyright notice,
* this list of conditions and the following disclaimer in the documentation
* and/or other materials provided with the distribution.
* * Neither the name of the Organizations nor the names of Individual
* Contributors may be used to endorse or promote products derived from
* this software without specific prior written permission.
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

using AuroraWeb.Utils;

using Environment = AuroraWeb.Environment;


namespace AuroraWeb
{
    public class WifiForgotPasswordGetHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiForgotPasswordGetHandler(WebApp webapp) :
            base("GET", "/wifi/forgotpassword")
        {
            m_WebApp = webapp;
        }

        public byte[] Handle(string path, Stream requestData,
                RequestFactory.IOSHttpRequest httpRequest, WifiConsoleHandler.IOSHttpResponse httpResponse)
        {
            httpResponse.ContentType = "text/html";

            Request request = RequestFactory.CreateRequest(string.Empty, httpRequest);
            AuroraWeb.Environment env = new AuroraWeb.Environment(request);

            string result = m_WebApp.Services.ForgotPasswordGetRequest(env);

            return WebAppUtils.StringToBytes(result);
        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }
    }

    public class WifiForgotPasswordPostHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiForgotPasswordPostHandler(WebApp webapp) :
            base("POST", "/wifi/forgotpassword")
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
            try
            {
                Dictionary<string, object> request =
                        WebUtils.ParseQueryString(body);

                string email = String.Empty;
                if (request.ContainsKey("email"))
                    email = request["email"].ToString();

                Request req = RequestFactory.CreateRequest(string.Empty, httpRequest);
                AuroraWeb.Environment env = new AuroraWeb.Environment(req);

                string result = m_WebApp.Services.ForgotPasswordPostRequest(env, email);

                return WebAppUtils.StringToBytes(result);
            }
            catch (Exception e)
            {
                m_log.DebugFormat("[FORGOT PASSWORD POST HANDLER]: Exception {0}", e);
            }

            return WebAppUtils.FailureResult();
        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }
    }

    public class WifiPasswordRecoverGetHandler : BaseStreamHandler
    {
        private static readonly ILog m_Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private WebApp m_WebApp;

        public WifiPasswordRecoverGetHandler(WebApp webapp) :
            base("GET", "/wifi/recover")
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
            try
            {
                string[] pars = SplitParams(path);
                if (pars.Length == 1) // /wifi/recover/token?email=email
                {
                    string token = pars[0];
                    if (httpRequest.Query.ContainsKey("email") && httpRequest.Query["email"] != null)
                    {
                        string email = HttpUtility.UrlDecode(httpRequest.Query["email"].ToString());
                        Request req = RequestFactory.CreateRequest(string.Empty, httpRequest);
                        AuroraWeb.Environment env = new AuroraWeb.Environment(req);

                        string result = m_WebApp.Services.RecoverPasswordGetRequest(env, email, token);

                        return WebAppUtils.StringToBytes(result);
                    }
                    else
                        m_Log.DebugFormat("[PASSWORD RECOVER GET HANDLER]: Query part does not contain email variable");
                }
                else
                    m_Log.DebugFormat("[PASSWORD RECOVER GET HANDLER]: Path does not have 1 params, it has {0}", pars.Length);
            }
            catch (Exception e)
            {
                m_Log.DebugFormat("[PASSWORD RECOVER GET HANDLER]: Exception {0}", e);
            }

            return WebAppUtils.FailureResult();
        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }
    }

    public class WifiPasswordRecoverPostHandler : BaseStreamHandler
    {
        private static readonly ILog m_Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private WebApp m_WebApp;

        public WifiPasswordRecoverPostHandler(WebApp webapp) :
            base("POST", "/wifi/recover")
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
            try
            {
                Dictionary<string, object> request =
                        WebUtils.ParseQueryString(body);

                string token = String.Empty;
                if (request.ContainsKey("token"))
                    token = request["token"].ToString();
                string email = String.Empty;
                if (request.ContainsKey("email"))
                    email = request["email"].ToString();

                string newPassword = String.Empty;
                if (request.ContainsKey("newpassword"))
                    newPassword = request["newpassword"].ToString();

                Request req = RequestFactory.CreateRequest(string.Empty, httpRequest);
                AuroraWeb.Environment env = new AuroraWeb.Environment(req);

                string result = m_WebApp.Services.RecoverPasswordPostRequest(env, email, token, newPassword);
                return WebAppUtils.StringToBytes(result);
            }
            catch (Exception e)
            {
                m_Log.DebugFormat("[FORGOT PASSWORD POST HANDLER]: Exception {0}", e);
            }

            return WebAppUtils.FailureResult();
        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }
    }


}