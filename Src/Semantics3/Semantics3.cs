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
using System.Net;
using System.Net.Http;
using System.Text;
using Semantics3.OAuth;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Semantics3
{
    public class Semantics3
    {
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
        /// <param name="_custom_api_base">Override the base API url.</param>
        /// <returns>void.</returns>
        public Semantics3(String apiKey, String apiSecret, String _custom_api_base = "")
        {
            api_domain = "api.semantics3.com";
            api_base = "https://" + api_domain + "/v1/";

            // If custom api base sent in, then use that
            if (!string.IsNullOrEmpty(_custom_api_base))
            {
                api_base = _custom_api_base;
            }

            oauth_client = new OAuth2LeggedAuthenticator(apiKey, apiSecret);
        }

        public async Task<string> fetch(String endpoint, String parameters, HttpMethod method)
        {
            String url = api_base + endpoint;

            // If method is GET, then send parameters as part of URL
            if (method == HttpMethod.Get)
            {
#if NET40
                var encodedParameters = Uri.EscapeUriString(parameters);
#else
                var encodedParameters = WebUtility.UrlEncode(parameters);
#endif
                url = url + "?q=" + encodedParameters;
            }

            _serviceProviderUri = new Uri(url);

            var request = oauth_client.CreateHttpWebRequest(method, _serviceProviderUri);

            // If not GET request, send parameters as request content
            if (method != HttpMethod.Get && parameters.Length > 0)
            {
                var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8,
                    "application/json");
                request.Content = content;
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
