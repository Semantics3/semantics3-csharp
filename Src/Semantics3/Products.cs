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
 * Name:     Products.cs
 * Project:  Semantics3 API C# Client
 * Purpose:  Class to query Semantics3 Product API products Endpoint.
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
    public class Products : Common
    {
        private int MAX_LIMIT = 10;

        /// <summary>
        /// Constructs the Products object by initializing the Base Class Common by sending the parameters.
        /// This Class queries the Products Endpoint of the Semantics3 API.
        /// Example:
        /// Products products = new Products("API_KEY", "API_SECRET");
        /// </summary>
        /// <param name="apiKey">API Key from semantics3.com</param>
        /// <param name="apiSecret">API Secret from semantics3.com</param>
        /// <returns>void.</returns>
        
        public Products(String apiKey, String apiSecret) : base(apiKey, apiSecret)
        {
        }

        /// <summary>
        /// Build up the query parameters before querying the Products Endpoint.
        /// products_field( params1, params2, ... )
        /// Example:
        /// products.products_field( 'cat_id', 4992 );
        /// products.products_field( 'brand', 'Toshiba' );
        /// products.products_field( 'weight', 'gte', 1000000 );
        /// products.products_field( 'weight', 'lt', 1500000 );
        /// </summary>
        /// <param name="data">Construct complex queries by sending parameters and values.</param>
        /// <returns>void.</returns>
        
        public void products_field(params object[] data)
        {
            add("products", data);
        }

        /// <summary>
        /// Returns the output of the query on the 'products' endpoint. 
        /// Note: The output of this method would overwrite the output of any 
        /// previous query stored in the results buffer.
        /// Example:
        /// JObject apiResponse = products.get_products();
        /// </summary>        
        /// <returns>A JObject Class object (from Newtonsoft.Json Lib) consisting of the parsed JSON response from the API.</returns>
        
        public JObject get_products()
        {
            return run_query("products", null);
        }

        /// <summary>
        /// Build up the query parameters before querying the Categories Endpoint.
        /// categories_field( params1, params2, ... )
        /// Example:
        /// products.categories_field( 'parent_cat_id', 1 );
        /// </summary>
        /// <param name="data">Construct complex queries by sending parameters and values.</param>
        /// <returns>void.</returns>

        public void categories_field(params object[] data)
        {
            add("categories", data);
        }

        /// <summary>
        /// Returns the output of the query on the 'categories' endpoint. 
        /// Note: The output of this method would overwrite the output of any 
        /// previous query stored in the results buffer.
        /// Example:
        /// JObject apiResponse = products.get_categories();
        /// </summary>        
        /// <returns>A JObject Class object (from Newtonsoft.Json Lib) consisting of the parsed JSON response from the API.</returns>

        public JObject get_categories()
        {
            return run_query("categories", null);
        }

        /// <summary>
        /// Build up the query parameters before querying the Offers Endpoint.
        /// offers_field( params1, params2, ... )
        /// Example:
        /// products.offers_field( 'currency', 'USD' );
        /// products.offers_field( 'currency', 'price', 'gte', 30 );
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
        /// JObject apiResponse = products.get_offers();
        /// </summary>        
        /// <returns>A JObject Class object (from Newtonsoft.Json Lib) consisting of the parsed JSON response from the API.</returns>

        public JObject get_offers()
        {
            return run_query("offers", null);
        }

        /// <summary>
        /// Converts and returns the results of the query output of the 'products' endpoint as an array reference.
        /// get_products() or run_query("products") must have been called before and should have returned a valid
        /// result set.
        /// Example:
        /// JArray arrayProducts = products.all_products();
        /// foreach (JToken product in arrayProducts)
        /// {
        ///     Console.Write(product.ToString());
        /// }
        /// </summary>        
        /// <returns>A JArray object (from Newtonsoft.Json Lib) consisting of an array of product results from the API.</returns>
        
        public JArray all_products()
        {            
            if (query_result["results"] == null)
            {
                throw new Error("Undefined Query",
                                "Query result is undefined. You need to run a query first. ",
                                "results");
            }
            JArray arrRef = query_result["results"] as JArray;
            return arrRef;
        }

        /// <summary>
        /// Returns the output of the query by auto incrementing the offset based on the limit defined.
        /// If limit is not defined, it defaults to default value of 10.
        /// Example:
        /// products.get_products();
        /// JObjects nextProducts = products.iterate_products();
        /// while( nextProductsRef != null ) 
        /// {
        ///     Console.Write(nextProducts.ToString());
        ///     nextProducts = products.iterate_products();
        /// }
        /// </summary>        
        /// <returns>A JObject consisting of an array of product after iterating once from the API.</returns>
        
        public JObject iterate_products()
        {
            int limit = MAX_LIMIT;

            if (query_result["products"]["total_results_count"] == null || (int)query_result["products"]["offset"] >= (int)query_result["products"]["total_results_count"])
            {
                return null;
            }

            int prevOffset = 0;
            if( data_query["products"]["offset"] != null ) 
            {
                prevOffset = (int)data_query["products"]["offset"];
            }
            if (data_query["products"]["limit"] != null)
            {
                limit = (int)data_query["products"]["limit"];
            }
            data_query["products"]["offset"] = prevOffset + limit;

            return get_products();
        }
    }
}
