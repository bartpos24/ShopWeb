# ShopApi.Client.Api.ProductApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ApiProductAddProductPost**](ProductApi.md#apiproductaddproductpost) | **POST** /api/Product/AddProduct |  |
| [**ApiProductGetAllProductsGet**](ProductApi.md#apiproductgetallproductsget) | **GET** /api/Product/GetAllProducts |  |
| [**ApiProductGetAllProductsTestOpenApiGenerateGet**](ProductApi.md#apiproductgetallproductstestopenapigenerateget) | **GET** /api/Product/GetAllProductsTestOpenApiGenerate |  |
| [**ApiProductGetProductByBarcodeGet**](ProductApi.md#apiproductgetproductbybarcodeget) | **GET** /api/Product/GetProductByBarcode |  |
| [**ApiProductGetProductFromOpenFoodFactsGet**](ProductApi.md#apiproductgetproductfromopenfoodfactsget) | **GET** /api/Product/GetProductFromOpenFoodFacts |  |
| [**ApiProductInitProductFromCSVPost**](ProductApi.md#apiproductinitproductfromcsvpost) | **POST** /api/Product/InitProductFromCSV |  |
| [**ApiProductTestApiDirectlyGet**](ProductApi.md#apiproducttestapidirectlyget) | **GET** /api/Product/TestApiDirectly |  |

<a id="apiproductaddproductpost"></a>
# **ApiProductAddProductPost**
> void ApiProductAddProductPost (Product? product = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ShopApi.Client.Api;
using ShopApi.Client.Client;
using ShopApi.Client.Model;

namespace Example
{
    public class ApiProductAddProductPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: Bearer
            config.AddApiKey("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Authorization", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ProductApi(httpClient, config, httpClientHandler);
            var product = new Product?(); // Product? |  (optional) 

            try
            {
                apiInstance.ApiProductAddProductPost(product);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProductApi.ApiProductAddProductPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiProductAddProductPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiProductAddProductPostWithHttpInfo(product);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProductApi.ApiProductAddProductPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **product** | [**Product?**](Product?.md) |  | [optional]  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiproductgetallproductsget"></a>
# **ApiProductGetAllProductsGet**
> void ApiProductGetAllProductsGet ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ShopApi.Client.Api;
using ShopApi.Client.Client;
using ShopApi.Client.Model;

namespace Example
{
    public class ApiProductGetAllProductsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: Bearer
            config.AddApiKey("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Authorization", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ProductApi(httpClient, config, httpClientHandler);

            try
            {
                apiInstance.ApiProductGetAllProductsGet();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProductApi.ApiProductGetAllProductsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiProductGetAllProductsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiProductGetAllProductsGetWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProductApi.ApiProductGetAllProductsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiproductgetallproductstestopenapigenerateget"></a>
# **ApiProductGetAllProductsTestOpenApiGenerateGet**
> void ApiProductGetAllProductsTestOpenApiGenerateGet ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ShopApi.Client.Api;
using ShopApi.Client.Client;
using ShopApi.Client.Model;

namespace Example
{
    public class ApiProductGetAllProductsTestOpenApiGenerateGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: Bearer
            config.AddApiKey("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Authorization", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ProductApi(httpClient, config, httpClientHandler);

            try
            {
                apiInstance.ApiProductGetAllProductsTestOpenApiGenerateGet();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProductApi.ApiProductGetAllProductsTestOpenApiGenerateGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiProductGetAllProductsTestOpenApiGenerateGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiProductGetAllProductsTestOpenApiGenerateGetWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProductApi.ApiProductGetAllProductsTestOpenApiGenerateGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiproductgetproductbybarcodeget"></a>
# **ApiProductGetProductByBarcodeGet**
> Collection&lt;Product&gt; ApiProductGetProductByBarcodeGet (string? barcode = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ShopApi.Client.Api;
using ShopApi.Client.Client;
using ShopApi.Client.Model;

namespace Example
{
    public class ApiProductGetProductByBarcodeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: Bearer
            config.AddApiKey("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Authorization", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ProductApi(httpClient, config, httpClientHandler);
            var barcode = "barcode_example";  // string? |  (optional) 

            try
            {
                Collection<Product> result = apiInstance.ApiProductGetProductByBarcodeGet(barcode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProductApi.ApiProductGetProductByBarcodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiProductGetProductByBarcodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<Collection<Product>> response = apiInstance.ApiProductGetProductByBarcodeGetWithHttpInfo(barcode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProductApi.ApiProductGetProductByBarcodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **barcode** | **string?** |  | [optional]  |

### Return type

[**Collection&lt;Product&gt;**](Product.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiproductgetproductfromopenfoodfactsget"></a>
# **ApiProductGetProductFromOpenFoodFactsGet**
> Product ApiProductGetProductFromOpenFoodFactsGet (string? barcode = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ShopApi.Client.Api;
using ShopApi.Client.Client;
using ShopApi.Client.Model;

namespace Example
{
    public class ApiProductGetProductFromOpenFoodFactsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: Bearer
            config.AddApiKey("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Authorization", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ProductApi(httpClient, config, httpClientHandler);
            var barcode = "barcode_example";  // string? |  (optional) 

            try
            {
                Product result = apiInstance.ApiProductGetProductFromOpenFoodFactsGet(barcode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProductApi.ApiProductGetProductFromOpenFoodFactsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiProductGetProductFromOpenFoodFactsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<Product> response = apiInstance.ApiProductGetProductFromOpenFoodFactsGetWithHttpInfo(barcode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProductApi.ApiProductGetProductFromOpenFoodFactsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **barcode** | **string?** |  | [optional]  |

### Return type

[**Product**](Product.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiproductinitproductfromcsvpost"></a>
# **ApiProductInitProductFromCSVPost**
> void ApiProductInitProductFromCSVPost ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ShopApi.Client.Api;
using ShopApi.Client.Client;
using ShopApi.Client.Model;

namespace Example
{
    public class ApiProductInitProductFromCSVPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: Bearer
            config.AddApiKey("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Authorization", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ProductApi(httpClient, config, httpClientHandler);

            try
            {
                apiInstance.ApiProductInitProductFromCSVPost();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProductApi.ApiProductInitProductFromCSVPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiProductInitProductFromCSVPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiProductInitProductFromCSVPostWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProductApi.ApiProductInitProductFromCSVPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiproducttestapidirectlyget"></a>
# **ApiProductTestApiDirectlyGet**
> void ApiProductTestApiDirectlyGet ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ShopApi.Client.Api;
using ShopApi.Client.Client;
using ShopApi.Client.Model;

namespace Example
{
    public class ApiProductTestApiDirectlyGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: Bearer
            config.AddApiKey("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Authorization", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ProductApi(httpClient, config, httpClientHandler);

            try
            {
                apiInstance.ApiProductTestApiDirectlyGet();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProductApi.ApiProductTestApiDirectlyGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiProductTestApiDirectlyGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiProductTestApiDirectlyGetWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProductApi.ApiProductTestApiDirectlyGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

