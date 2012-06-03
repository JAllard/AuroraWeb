/**
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

using AuroraWeb.Utils;

using Environment = AuroraWeb.Environment;


namespace AuroraWeb
{
    public class WifiConsoleHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebApp m_WebApp;

        public WifiConsoleHandler(WebApp webapp) :
            base("GET", "/wifi/admin/console")
        {
            m_WebApp = webapp;
        }

        public interface IOSHttpResponse
        {
            /// <summary>
            /// Content type property.
            /// </summary>
            /// <remarks>
            /// Setting this property will also set IsContentTypeSet to
            /// true.
            /// </remarks>
            string ContentType { get; set; }

            /// <summary>
            /// Boolean property indicating whether the content type
            /// property actively has been set.
            /// </summary>
            /// <remarks>
            /// IsContentTypeSet will go away together with .NET base.
            /// </remarks>
            // public bool IsContentTypeSet
            // {
            //     get { return _contentTypeSet; }
            // }
            // private bool _contentTypeSet;

            /// <summary>
            /// Length of the body content; 0 if there is no body.
            /// </summary>
            long ContentLength { get; set; }

            /// <summary>
            /// Alias for ContentLength.
            /// </summary>
            long ContentLength64 { get; set; }

            /// <summary>
            /// Encoding of the body content.
            /// </summary>
            Encoding ContentEncoding { get; set; }

            bool KeepAlive { get; set; }

            /// <summary>
            /// Get or set the keep alive timeout property (default is
            /// 20). Setting this to 0 also disables KeepAlive. Setting
            /// this to something else but 0 also enable KeepAlive.
            /// </summary>
            int KeepAliveTimeout { get; set; }

            /// <summary>
            /// Return the output stream feeding the body.
            /// </summary>
            /// <remarks>
            /// On its way out...
            /// </remarks>
            Stream OutputStream { get; }

            string ProtocolVersion { get; set; }

            /// <summary>
            /// Return the output stream feeding the body.
            /// </summary>
            Stream Body { get; }

            /// <summary>
            /// Set a redirct location.
            /// </summary>
            string RedirectLocation { set; }

            /// <summary>
            /// Chunk transfers.
            /// </summary>
            bool SendChunked { get; set; }

            /// <summary>
            /// HTTP status code.
            /// </summary>
            int StatusCode { get; set; }

            /// <summary>
            /// HTTP status description.
            /// </summary>
            string StatusDescription { get; set; }

            bool ReuseContext { get; set; }

            /// <summary>
            /// Add a header field and content to the response.
            /// </summary>
            /// <param name="key">string containing the header field
            /// name</param>
            /// <param name="value">string containing the header field
            /// value</param>
            void AddHeader(string key, string value);

            /// <summary>
            /// Send the response back to the remote client
            /// </summary>
            void Send();
        }
        public byte[] Handle(string path, Stream requestData,
                RequestFactory.IOSHttpRequest httpRequest, IOSHttpResponse httpResponse)
        {
            string result = string.Empty;
            try
            {
                Request request = RequestFactory.CreateRequest(string.Empty, httpRequest);
                AuroraWeb.Environment env = new AuroraWeb.Environment(request);

                string resource = GetParam(path);
                //m_log.DebugFormat("[XXX]: resource {0}", resource);
                if (resource.StartsWith("/data/simulators"))
                {
                    result = m_WebApp.Services.ConsoleSimulatorsRequest(env);
                    httpResponse.ContentType = "application/xml";
                }
                else if (resource.StartsWith("/heartbeat"))
                {
                    result = m_WebApp.Services.ConsoleHeartbeat(env);
                    httpResponse.ContentType = "application/xml";
                }
                else
                {
                    result = m_WebApp.Services.ConsoleRequest(env);
                    httpResponse.ContentType = "text/html";
                }
            }
            catch (Exception e)
            {
                m_log.DebugFormat("[CONSOLE HANDLER]: Exception {0}: {1}", e.Message, e.StackTrace);
            }

            return WebAppUtils.StringToBytes(result);
        }

        public override byte[] Handle(string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }
    }
}
