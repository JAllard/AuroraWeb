/**
 * Copyright (c) Marcus Kirsch (aka Marck), Crista Lopes (aka Diva), JAllard, Enrico Nirvana. All rights reserved.
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
using log4net;
using System;
using System.Reflection;
using System.IO;

using AuroraWeb.Utils;

namespace AuroraWeb
{
    public class WifiGetHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string m_LocalPath;

        public WifiGetHandler(string localPath, string resourcePath) :
            base("GET", resourcePath)
        {
            m_LocalPath = localPath;
            m_log.InfoFormat("[AuroraWeb]: Serving local path {0} via resource path {1}", localPath, resourcePath);
        }
        
        public byte[] Handle(string path, Stream requestData,
                RequestFactory.IOSHttpRequest httpRequest, WifiConsoleHandler.IOSHttpResponse httpResponse)
        {
            string resource = GetParam(path);
            //m_log.DebugFormat("[AuroraWeb]: resource {0}", resource);
            resource = resource.Trim(WebAppUtils.DirectorySeparatorChars);
            string resourcePath = System.IO.Path.Combine(m_LocalPath, resource);
            resourcePath = Uri.UnescapeDataString(resourcePath);

            string type = WebAppUtils.GetContentType(resource);
            httpResponse.ContentType = type;
            //m_log.DebugFormat("[AuroraWeb]: ContentType {0}", type);
            if (type.StartsWith("image"))
                return WebAppUtils.ReadBinaryResource(resourcePath);

            if (type.StartsWith("application") || type.StartsWith("text"))
            {
                string res = WebAppUtils.ReadTextResource(resourcePath, true);
                return WebAppUtils.StringToBytes(res);
            }

            m_log.WarnFormat("[AuroraWeb]: Could not find resource {0} in local path {1}", resource, m_LocalPath);
            httpResponse.ContentType = "text/plain";
            string result = "Boo!";
            return WebAppUtils.StringToBytes(result);
        }


        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            string resource = GetParam(path);
            //m_log.DebugFormat("[AuroraWeb]: resource {0}", resource);
            resource = resource.Trim(WebAppUtils.DirectorySeparatorChars);
            string resourcePath = System.IO.Path.Combine(m_LocalPath, resource);
            resourcePath = Uri.UnescapeDataString(resourcePath);

            string type = WebAppUtils.GetContentType(resource);
            httpResponse.ContentType = type;
            //m_log.DebugFormat("[AuroraWeb]: ContentType {0}", type);
            if (type.StartsWith("image"))
                return WebAppUtils.ReadBinaryResource(resourcePath);

            if (type.StartsWith("application") || type.StartsWith("text"))
            {
                string res = WebAppUtils.ReadTextResource(resourcePath, true);
                return WebAppUtils.StringToBytes(res);
            }

            m_log.WarnFormat("[AuroraWeb]: Could not find resource {0} in local path {1}", resource, m_LocalPath);
            httpResponse.ContentType = "text/plain";
            string result = "Boo!";
            return WebAppUtils.StringToBytes(result);
        }
    }
}
