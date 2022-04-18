using Microsoft.AspNetCore.Http;

namespace SaturdayQuizWeb.Extensions;

public static class HttpResponseExtensions
{
    public static void AddCustomHeaders(this HttpResponse response)
    {
        response.Headers.CacheControl = "no-cache";
        response.Headers.XContentTypeOptions = "nosniff";
    }
}
