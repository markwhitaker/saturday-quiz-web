namespace SaturdayQuizWeb.Extensions;

public static class HttpResponseExtensions
{
    public static void AddCustomHeaders(this HttpResponse response, TimeSpan cacheDuration)
    {
        response.Headers.CacheControl = cacheDuration.ToCacheControlHeaderValue();
        response.Headers.XContentTypeOptions = "nosniff";
        response.Headers.AccessControlAllowOrigin = "*";
    }
}
