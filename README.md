# semantics3-csharp

semantics3-csharp is a C# client for accessing the Semantics3 Products API, which provides structured information, including pricing histories, for a large number of products.
See https://www.semantics3.com for more information.

API documentation can be found at https://www.semantics3.com/docs/

## Installation

semantics3-csharp can be installed through the Nuget. Nuget: https://www.nuget.org/packages/Semantics3/.
Target Frameworks for this library build are:
 * .NET Framework 4.0;
 * .NET Framework 4.5;
 * .NET Core 1.0;

Change your Target Framework from .NET Framework 4.0 (or 4.5) Client Profile to just .NET Framework 4.0 (or 4.5) in your project properties if need to.

```bash
$ Install-Package Semantics3
```
To build and install from the latest source:
```bash
$ git clone git@github.com:Semantics3/semantics3-csharp.git
```

## Requirements

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

### First Query aka 'Hello World':

Let's run our first request! We are going to run a simple search fo the word "iPhone" as follows:

```c#
// Build the query
products.products_field( "search", "iphone" );

// Make the query
JObject hashProducts = products.get_products();

// View the results of the query
Console.Write(hashProducts.ToString());
```

## Sample Requests

The following requests show you how to interface with some of the core functionality of the
Semantics3 Products API:

### Pagination

The example in our "Hello World" script returns the first 10 results. In this example, we'll scroll to subsequent pages, beyond our initial request:

```c#
// Build the query
products.products_field( "search", "iphone" );

// Make the query
JObject hashProducts = products.get_products();

// View the results of the query
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
// Build the query
products.products_field( "url", "http://www.walmart.com/ip/15833173" );

// Make the query
JObject hashProducts = products.get_products();

// View the results of the query
Console.Write(hashProducts.ToString());
```

### Price Filter

Filter by price using the "lt" (less than) tag:

```c#
// Build the query
products.products_field( "search", "iphone" );
products.products_field( "price", "lt", 300 );

// Make the query
JObject hashProducts = products.get_products();

// View the results of the query
Console.Write(hashProducts.ToString());
```

### Category ID Query

To lookup details about a cat_id, run your request against the categories resource:

```c#
// Build the query
products.categories_field( "cat_id", 4992 );

// Make the query
JObject hashCategories = products.get_categories();

// View the results of the query
Console.Write(hashCategories.ToString());
```

### Parse JSON/Response Example

The code below shows how to use the JObject (from Newtonsoft.Json library) value returned from the Semantics3 API to access the data fields interested in. The JSON response from the API is already parsed into the Newtonsoft.Json Object ready to be consumed easily in your target application.

```c#

// Query the API
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
// If the above query resulted in some results

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
        ' Build the query
        products.products_field("search", "iphone")
        Dim constructedJson As String = products.get_query_json("products")
        Console.Write(constructedJson)

        ' Make the query
        Dim apiResponse As JObject = products.get_products()
        Console.Write(apiResponse.ToString())

    End Sub

End Module

```

## Webhooks
You can use webhooks to get near-real-time price updates from Semantics3.

### Creating a webhook

You can register a webhook with Semantics3 by sending a POST request to `"webhooks"` endpoint.
To verify that your URL is active, a GET request will be sent to your server with a `verification_code` parameter. Your server should respond with `verification_code` in the response body to complete the verification process.

If your webhook server does not respond to as indicated in the Semantics3 Webhooks documentation, the run_query
call will crash. You'll have to surround them in a try catch block.


```c#
JObject addWebhookUriQuery = new JObject();
addWebhookUriQuery["webhook_uri"] = "http://mydomain.com/myendpoint";
JObject hashResponse = products.run_query("webhooks", addWebhookUriQuery, "POST");
Console.Write(hashResponse.ToString());
```

To fetch existing webhooks

```c#
JObject hashResponse = products.run_query("webhooks", "{}", "GET");
Console.Write(hashResponse.ToString());
```

To remove a webhook

```c#
String webhookId = "7JcGN81u";
String endpoint = "webhooks/" + webhookId;
JObject hashResponse = products.run_query( endpoint, "{}", "DELETE" );
Console.Write(hashResponse.ToString());
```

### Registering events
Once you register a webhook, you can start adding events to it. Semantics3 server will send you notifications when these events occur.
To register events for a specific webhook send a POST request to the `"webhooks/{webhook_id}/events"` endpoint

```c#
JObject productSpec = new JObject();
productSpec["sem3_id"] = "1QZC8wchX62eCYS2CACmka";

JObject constraintsSpec = new JObject();
constraintsSpec["gte"] = 10;
constraintsSpec["lte"] = 100;

JObject createEventsQuery = new JObject();
createEventsQuery["type"] = "price.change";
createEventsQuery["product"] = productSpec;
createEventsQuery["constraints"] = constraintsSpec;

String webhookId = "7JcGN81u";
String endpoint = "webhooks/" + webhookId + "/events";
JObject hashResponse = products.run_query( endpoint, createEventsQuery, "POST" );
Console.Write(hashResponse.ToString());
```

To fetch all registered events for a give webhook

```c#
String webhookId = "7JcGN81u";
String endpoint = "webhooks/" + webhookId + "/events";

JObject hashResponse = products.run_query(endpoint, "{}", "GET");
Console.Write(hashResponse.ToString());
```

### Webhook Notifications

Once you have created a webhook and registered events on it, notifications will be sent to your registered webhook URI via a POST request when the corresponding events occur. Make sure that your server can accept POST requests. Here is how a sample notification object looks like

```javascript
{
    "type": "price.change",
    "event_id": "XyZgOZ5q",
    "notification_id": "X4jsdDsW",
    "changes": [{
        "site": "abc.com",
        "url": "http://www.abc.com/def",
        "previous_price": 45.50,
        "current_price": 41.00
    }, {
        "site": "walmart.com",
        "url": "http://www.walmart.com/ip/20671263",
        "previous_price": 34.00,
        "current_price": 42.00
    }]
}
```

### Additional utility methods

| method        | Description
| ------------- |:-------------
| `products.get_results_json()`     | returns the result json string from the previous query
| `products.clear()`                | clears all the fields in the query
| `products.run_query(endpoint, rawJson, method, callback)`  | You can use this method to send raw JSON string in the request


## Contributing

Use GitHub's standard fork/commit/pull-request cycle.  If you have any questions, email <support@semantics3.com>.

## Author

* Srinivasan Kidambi <srinivas@semantics3.com>

## Copyright

Copyright (c) 2014 Semantics3 Inc.

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
