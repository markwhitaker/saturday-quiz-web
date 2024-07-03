﻿using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients.HttpClients;

public class GuardianWebsiteHttpClient(HttpClient httpClient, IOptions<GuardianConfig> configOptions)
    : IGuardianWebsiteHttpClient
{
    public async Task<string> GetStringAsync(string endpoint)
    {
        httpClient.BaseAddress ??= new Uri(configOptions.Value.WebsiteBaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        return await httpClient.GetStringAsync(endpoint);
    }
}
