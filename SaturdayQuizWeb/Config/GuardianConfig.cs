namespace SaturdayQuizWeb.Config;

public record GuardianConfig
{
    // Maps to Guardian__ApiKey in environment variables
    public string ApiKey { get; init; } = string.Empty;

    public string ApiBaseUrl { get; init; } = string.Empty;

    public string ApiEndpoint { get; init; } = string.Empty;

    public string RssEndpoint { get; init; } = string.Empty;

    public string WebsiteBaseUrl { get; init; } = string.Empty;
}
