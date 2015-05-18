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
 * Name:     Common.cs
 * Project:  Semantics3 API C# Client
 * Purpose:  Class to query any Semantics3 API Endpoint.
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
    public class Common : Semantics3
    {
        protected JObject data_query;
        protected JObject query_result;

        /// <summary>
        /// Constructs the Products object by initializing the Base Class Semantics3 by sending the parameters.
        /// This Class is a base class to query any Endpoint offered by the Semantics3 API
        /// Example:
        /// Common anyEndpoint = new Common("API_KEY", "API_SECRET");
        /// </summary>
        /// <param name="apiKey">API Key from semantics3.com</param>
        /// <param name="apiSecret">API Secret from semantics3.com</param>
        /// <returns>void.</returns>
       
        public Common(String apiKey, String apiSecret) : base(apiKey, apiSecret)
        {
            data_query = new JObject();
            query_result = new JObject();
        }

        /// <summary>
        /// Build up the query parameters before querying any Endpoint.
        /// add( ENDPOINT, params1, params2, ... )
        /// Example:
        /// anyEndpoint.add("products","cat_id",4992);
        /// anyEndpoint.add("products","brand","Toshiba");
        /// anyEndpoint.add("products","weight","gte",1000000);
        /// anyEndpoint.add("products","weight","lt",1500000);
        /// </summary>
        /// <param name="data">Construct complex queries by sending parameters and values.</param>
        /// <returns>void.</returns>
        
        public void add(String endpoint, params object[] data)
        {
            checkEndpoint(endpoint);
            data_query[endpoint] = editHash("add", (JObject)data_query[endpoint], data);
        }

        /// <summary>
        /// Removes any specific parameters in the constructured query parameter set for any endpoint.
        /// remove( ENDPOINT, params1, params2, ... )
        /// Example:
        /// Removes the 'gte' attribute and it's associated weight
        /// anyEndpoint.remove( 'products', 'weight', 'gte' );
        /// Removes the entire 'brand' field from the query
        /// $sem3->remove( 'products', 'brand' );     
        /// </summary>
        /// <param name="endpoint">Semantics3 API Endpoint to query to.</param>
        /// <param name="data">Parameters to remove from the query.</param>
        /// <returns>void.</returns>
        
        public void remove(String endpoint, params object[] data)
        {
            checkEndpoint(endpoint);
            data_query[endpoint] = editHash("del", (JObject)data_query[endpoint], data);
        }

        /// <summary>
        /// Returns the JObject hash object of the constructed query for the specified endpoint.
        /// get_query( ENDPOINT )
        /// Example:
        /// JObject hashQuery = anyEndpoint.get_query('products');   
        /// </summary>
        /// <param name="endpoint">Semantics3 API Endpoint.</param>
        /// <returns>A JObject Class object (from Newtonsoft.Json Lib) consisting of the query parameters (key, value).</returns>
        
        public JObject get_query(String endpoint)
        {
            checkEndpoint(endpoint);
            return (JObject)data_query[endpoint];
        }

        /// <summary>
        /// Returns the JSON string of the constructed query for the specified endpoint.
        /// get_query_json( ENDPOINT )
        /// Example:
        /// String jsonQuery = anyEndpoint.get_query_json('products');   
        /// </summary>
        /// <param name="endpoint">Semantics3 API Endpoint.</param>        
        /// <returns>String containing the JSON encoded query.</returns>
         
        public String get_query_json(String endpoint)
        {
            checkEndpoint(endpoint);
            return data_query[endpoint].ToString(Newtonsoft.Json.Formatting.None);
        }

        /// <summary>
        /// Returns the hash reference of the results from any previously executed query.
        /// get_results( endpoint )
        /// Example:
        /// JObject hashResults = anyEndpoint.get_results("products");   
        /// </summary> 
        /// <returns>A JObject Class object (from Newtonsoft.Json Lib) consisting of parsed JSON response from previous query.</returns>
        
        public JObject get_results(String endpoint)
        {
            return (JObject)query_result[endpoint];
        }

        /// <summary>
        /// Returns the JSON string of the results from any previously executed query.
        /// get_results_json( endpoint )
        /// Example:
        /// String jsonResults = anyEndpoint.get_results_json("products");   
        /// </summary> 
        /// <returns>String consisting of JSON response from previous query.</returns>
        
        public String get_results_json(String endpoint)
        {
            return query_result[endpoint].ToString(Newtonsoft.Json.Formatting.None);
        }

        /// <summary>
        /// Clears previously constructed parameters for each of the endpoints and also the results buffer.
        /// clear( )
        /// Example:
        /// anyEndpoint.clear();   
        /// </summary> 
        /// <returns>void.</returns>
        
        public void clear()
        {
            data_query = new JObject();
            query_result = new JObject();
        }

        /// <summary>
        /// Execute query of any endpoint based on the previously constructed query parameters or alternatively
        /// execute query of any endpoint based on the hash reference or JSON string of the query you wish to 
        /// supply. Returns a hash reference of the executed query.
        /// Example:
        /// Just query based on constructed query using methods add() or endpoint-specific methods like products_field(), etc..
        /// JObject hashResults = run_query( 'products' );
        /// Pass in a JObject hash containing query parameters: key=>value
        /// JObject hashResults = run_query( 'products', (JObject)queryParams );
        /// Pass in a JSON string
        /// JObject hashResults = run_query( 'products', "{\"cat_id\":4992,\"brand\":\"Toshiba\",\"weight\":{\"gte\":1000000,\"lt\":1500000}" );
        /// </summary> 
        /// <returns>void.</returns>
        
        public JObject run_query(String endpoint, object data, String method = "GET")
        {
            checkEndpoint(endpoint);

            if (data == null)
            {                
                query_result[endpoint] = JObject.Parse(fetch(endpoint, data_query[endpoint].ToString(Newtonsoft.Json.Formatting.None), method));
            }
            else
            {
                Type dataType = data.GetType(), strType = typeof(String), jobjectType = typeof(JObject);

                // Data is nether hash nor string
                if (dataType != strType && dataType != jobjectType)
                {
                    throw new Error("Invalid Data",
                                    "Invaid data was sent. Reference Type: - " + data.GetType(),
                                    "data");
                }
                // Data is Hash ref. Great just send it.
                else if (dataType == jobjectType)
                {                    
                    query_result[endpoint] = JObject.Parse(fetch(endpoint, ((JObject)data).ToString(Newtonsoft.Json.Formatting.None), method));
                }
                // Data is String
                else if (dataType == strType)
                {
                    // Check if it is valid JSON
                    try { JObject jb = JObject.Parse(data.ToString()); }
                    // Nope. Throw Error
                    catch (JsonReaderException e)
                    {
                        throw new Error("Invalid JSON: " + e.Message, 
                                        "Invalid JSON was sent.", 
                                        "data");
                    }

                    // Yup It's valid JSON. Just Send it.
                    query_result[endpoint] = JObject.Parse( fetch(endpoint, data.ToString(), method) );
                }
            }

            return (JObject)query_result[endpoint];
        }

        #region LocalHelperMethods

        private JObject editHash(String op, JObject dq, params object[] data)
        {
            if (dq == null)
            {
                dq = new JObject();
            }
            if (op == "del")
            {
                if (data.Length == 1)
                {
                    dq.Remove((String)data[0]);
                }
                else
                {
                    dq[data[0]] = (JToken)editHash(op,
                                                   (JObject)dq[data[0]],
                                                   data.Skip(1).Take(data.Length - 1).Cast<object>().ToArray());
                    if (dq[data[0]].Count() == 0)
                    {
                        dq.Remove((String)data[0]);
                    }
                }
            }
            else
            {
                if (data.Length == 2)
                {
                    if (data[1].GetType() == typeof(int))
                    {
                        dq[data[0]] = (int)data[1];
                    }
                    else if (data[1].GetType() == typeof(String))
                    {
                        dq[data[0]] = (String)data[1];
                    }
                    else if (data[1].GetType() == typeof(JArray) || data[1].GetType() == typeof(JObject))
                    {
                        dq[data[0]] = (JToken)data[1];
                    }
                }
                else
                {
                    dq[data[0]] = (JToken)editHash(op,
                                                   (JObject)dq[data[0]],
                                                   data.Skip(1).Take(data.Length - 1).Cast<object>().ToArray());
                }
            }
            return dq;
        }

        private void checkEndpoint(String endpoint)
        {
            if (endpoint == null || endpoint == "")
            {
                throw new Error("Undefined Endpoint",
                                "Query Endpoint was not defined. You need to provide one. Eg: products",
                                "endpoint");
            }
        }

        #endregion
    }
}
