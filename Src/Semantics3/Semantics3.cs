#region License
//The MIT License

//Copyright (c) 2013 Semantics3, Inc. https://semantics3.com

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
/******************************************************************************
 *
 * Name:     Semantics3.cs
 * Project:  Semantics3 API C# Client
 * Purpose:  C# Client to query Semantics3 APIs.
 * Author:   Srinivasan Kidambi, srinivas@semantics3.com
 *
 ******************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Net;
using System.Linq;
using System.Text;
using Google.GData.Client;

namespace Semantics3
{
    public class Semantics3
    {
        private readonly String api_key;
        private readonly String api_secret;
        private readonly String api_domain;
        private readonly String api_base;
        private Uri _serviceProviderUri;
        OAuth2LeggedAuthenticator oauth_client;

        /// <summary>
        /// Constructs the Semantics3 object by initializing the api key and secret.
        /// Base for API Client interfacing with the Semantics3 APIs
        /// Semantics3 sem3 = new Semantics3("API_KEY", "API_SECRET");
        /// </summary>
        /// <param name="apiKey">API Key from semantics3.com</param>
        /// <param name="apiSecret">API Secret from semantics3.com</param>
        /// <returns>void.</returns>
        
        public Semantics3(String consumerKey, String consumerSecret, String _custom_api_base = "")
        {
            api_domain = "api.semantics3.com";
            api_base = "https://" + api_domain + "/v1/";

            // If custom api base sent in, then use that
            if (_custom_api_base != null && _custom_api_base != "")
            {
                api_base = _custom_api_base;
            }

            api_key = consumerKey;
            api_secret = consumerSecret;
            oauth_client = new OAuth2LeggedAuthenticator("AppName", consumerKey, consumerSecret);
        }

        public String fetch(String endpoint, String parameters, String method)
        {
            String url = api_base + endpoint;

            // If method is GET, then send parameters as part of URL
            if (method == "GET")
            {
                url = url + "?q=" + System.Web.HttpUtility.UrlEncode(parameters);
            }

            _serviceProviderUri = new Uri(url);

            // HttpWebRequest request = GenerateRequest("application/json", "GET");
            HttpWebRequest request = oauth_client.CreateHttpWebRequest(method, _serviceProviderUri);
            
            // If not GET request, send parameters as request content
            if (method != "GET" && parameters.Length > 0)
            {
                var data = Encoding.ASCII.GetBytes(parameters);
                request.Method = method;
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      
            // Get the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            String result = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            return result;
        }
    }

    class OAuth2LeggedAuthenticator : OAuthAuthenticator
    {
        public OAuth2LeggedAuthenticator(string applicationName, string consumerKey, string consumerSecret)
            : base(applicationName, consumerKey, consumerSecret)
        {
        }

        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);
            string userAgent = "Semantics3 C# Lib/1.0.0.23";
            string header = OAuthUtil.GenerateHeader(request.RequestUri, ConsumerKey, ConsumerSecret, null, null, request.Method);
            request.Headers.Add(header);
            request.UserAgent = userAgent;
        }
    }
}
