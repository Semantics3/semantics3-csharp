# semantics3-csharp

semantics3-csharp is a C# client for accessing the Semantics3 Products API, which provides structured information, including pricing histories, for a large number of products.
See https://www.semantics3.com for more information.

API documentation can be found at https://www.semantics3.com/docs/

## Installation

semantics3-csharp can be installed through the Nuget. Nuget: https://www.nuget.org/packages/Semantics3/.
Target Frameworks for this library build are .NET Framework 4.0 and .NET Framework 4.5. Change your Target Framework from .NET Framework 4.0 (or 4.5) Client Profile to just .NET Framework 4.0 (or 4.5) in your project properties if need to.
```bash
$ Install-Package Semantics3
```
To build and install from the latest source:
```bash
$ git clone git@github.com:Semantics3/semantics3-csharp.git
```

## Requirements

* Google.GData.Client >= 2.2.2.0
* Newtonsoft.Json >= 5.0.6
* .NET Framework >= 4.0

## Getting Started

In order to use the client, you must have both an API key and an API secret. To obtain your key
and secret, you need to first create an account at
https://www.semantics3.com/

You can access your API access credentials from the user dashboard at
https://www.semantics3.com/dashboard/applications

### Setup Work

Let's lay the groundwork.

```c#
using Semantics3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Products products = new Products("APIKEY",
                                             "APISECRET");
        }
    }
}
```

### First Request aka 'Hello World':

Let's run our first request! We are going to run a simple search fo the word "iPhone" as follows:

```c#
// Build the request
products.products_field( "search", "iphone" );

// Make the request
JObject hashProducts = products.get_products();

// View results of the request
Console.Write(hashProducts.ToString());
```

## Sample Requests

The following requests show you how to interface with some of the core functionality of the
Semantics3 Products API:

### Pagination

The example in our "Hello World" script returns the first 10 results. In this example, we'll scroll to subsequent pages, beyond our initial request:

```c#
// Build the request
products.products_field( "search", "iphone" );

// Make the request
JObject hashProducts = products.get_products();

// View the results of the request
Console.Write(hashProducts.ToString());

// Goto the next 'page'
int page = 1;
JObject nextProducts = products.iterate_products();
while( nextProducts != null ) 
{
    Console.Write("We are at page"); Console.Write(page);
    Console.Write(nextProducts.ToString());
    page++;
    nextProducts = products.iterate_products();
}
```

### URL Query

Get the picture? You can run URL queries as follows:

```c#
// Build the request
products.products_field( "url", "http://www.walmart.com/ip/15833173" );

// Make the request
JObject hashProducts = products.get_products();

// View the results of the request
Console.Write(hashProducts.ToString());
```

### Price Filter

Filter by price using the "lt" (less than) tag:

```c#
// Build the request
products.products_field( "search", "iphone" );
products.products_field( "price", "lt", 300 );

// Make the request
JObject hashProducts = products.get_products();

// View the results of the request
Console.Write(hashProducts.ToString());
```

### Category ID Query

To lookup details about a cat_id, run your request against the categories resource:

```c#
// Build the request
products.categories_field( "cat_id", 4992 );

// Make the request
JObject hashCategories = products.get_categories();

// View the results of the request
Console.Write(hashCategories.ToString());
```

### Parse JSON/Response Example

The code below shows how to use the JObject (from Newtonsoft.Json library) value returned from the Semantics3 API to access the data fields interested in. The JSON response from the API is already parsed into the Newtonsoft.Json Object ready to be consumed easily in your target application.

```c#

// Send request to the API
JObject hashResponse = products.get_products();

if ((int)hashResponse["results_count"] > 0)
{
    JArray arrayProducts = (JArray)hashResponse["results"];

    // For each product in the results
    for (int i = 0; i < arrayProducts.Count; i++)
    {
        String productTitle = (String)arrayProducts[i]["name"];
        List<String> images = new List<string>();
        for (int j = 0; j < ((JArray)arrayProducts[i]["images"]).Count; j++)
        {
            images.Add((String)arrayProducts[i]["images"][j]);
        }

        // Pricing Information
        JArray ecommerceStores = (JArray)arrayProducts[i]["sitedetails"];
        for (int k = 0; k < ecommerceStores.Count; k++)
        {
            String skuInStore = (String)ecommerceStores[k]["sku"];
            String productUrlInStore = (String)ecommerceStores[k]["url"];

            // Latest offers from the ecommerceStore
            for (int s = 0; s < ((JArray)ecommerceStores[k]["latestoffers"]).Count; s++)
            {
                String sellerName = (String)ecommerceStores[k]["latestoffers"][s]["seller"];
                String price = (String)ecommerceStores[k]["latestoffers"][s]["price"];
                String currency = (String)ecommerceStores[k]["latestoffers"][s]["currency"];
                String availability = (String)ecommerceStores[k]["latestoffers"][s]["availability"];

                // Preprocess and use in application ...

            }
            // Parsed offers
        }
        // Loop Ecommerse stores

    }
    // Loop each product in the api response

}
// If the above request resulted in some results

```

### VB .NET

Install this library through Nuget package manager using the instructions above. After installing the library in your project (Make sure your target framework is .NET 4 (or later) and NOT .NET 4(or later) Client Framework , can be changed in Properties->Compile->Advanced Compile Options->Target Framework), here's a sample VB .NET code on using the API. All of the above library functions work the same in VB as the Semantics3 API C# library is CLS Compliant.

```vb
Imports Semantics3
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Module1

    Sub Main()
        Dim products As Products = New Products("API_KEY", "API_SECRET")
        ' Build the request
        products.products_field("search", "iphone")
        Dim constructedJson As String = products.get_query_json("products")
        Console.Write(constructedJson)

        ' Make the request
        Dim apiResponse As JObject = products.get_products()
        Console.Write(apiResponse.ToString())

    End Sub

End Module

```

## Contributing

Use GitHub's standard fork/commit/pull-request cycle.  If you have any questions, email <support@semantics3.com>.

## Author

* Srinivasan Kidambi <srinivas@semantics3.com>

## Copyright

Copyright (c) 2015 Semantics3 Inc.

## License

    The "MIT" License
    
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
    OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
    DEALINGS IN THE SOFTWARE.
