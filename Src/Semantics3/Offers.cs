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
 * Name:     Offers.cs
 * Project:  Semantics3 API C# Client
 * Purpose:  Class to query Offers Endpoint.
 * Author:   Srinivasan Kidambi, srinivas@semantics3.com
 *
 ******************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Semantics3
{
    public class Offers : Common
    {
        /// <summary>
        /// Constructs the Offers object by initializing the Base Class Common by sending the parameters.
        /// This Class queries the Offers Endpoint of the Semantics3 API
        /// Offers offers = new Offers("API_KEY", "API_SECRET");
        /// </summary>
        /// <param name="apiKey">API Key from semantics3.com</param>
        /// <param name="apiSecret">API Secret from semantics3.com</param>
        /// <returns>void.</returns>
        
        public Offers(String apiKey, String apiSecret) : base(apiKey, apiSecret)
        {
        }

        /// <summary>
        /// Build up the query parameters before querying the Offers Endpoint.
        /// offers_field( params1, params2, ... )
        /// Example:
        /// offers.offers_field( 'currency', 'USD' );
        /// offers.offers_field( 'currency', 'price', 'gte', 30 );
        /// </summary>
        /// <param name="data">Construct complex queries by sending parameters and values.</param>
        /// <returns>void.</returns>
        
        public void offers_field(params object[] data)
        {
            add("offers", data);
        }

        /// <summary>
        /// Returns the output of the query on the 'offers' endpoint. 
        /// Note: The output of this method would overwrite the output of any 
        /// previous query stored in the results buffer.
        /// Example:
        /// JObject apiResponse = offers.get_offers();
        /// </summary>        
        /// <returns>A JObject Class object (from Newtonsoft.Json Lib) consisting of the parsed JSON response from the API.</returns>
        
        public JObject get_offers()
        {
            return run_query("offers", null);
        }
    }
}
