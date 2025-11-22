using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Models;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianRssClient : IGuardianQuizMetadataClient;

public class GuardianRssClient(
    IOptions<GuardianConfig> guardianConfig,
    IGuardianWebsiteHttpClient guardianWebsiteHttpClient,
    ILogger<GuardianRssClient> logger)
    : IGuardianRssClient
{
    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        try
        {
            var config = guardianConfig.Value;
            var contents = await guardianWebsiteHttpClient.GetStringAsync(config.RssEndpoint);

            var xmlSerializer = new XmlSerializer(typeof(XmlRssRoot));
            using var reader = new StringReader(contents);
            var xmlRss = xmlSerializer.Deserialize(reader) as XmlRssRoot;
            return xmlRss!.Channel.Items
                .Select(item => new QuizMetadata
                {
                    Title = item.Title.Trim(),
                    Url = item.Link.Trim(),
                    Date = item.Date,
                    Id = item.Link.Trim().Replace(config.WebsiteBaseUrl, string.Empty),
                    Source = Constants.SourceRss
                })
                .Distinct()
                .OrderByDescending(qm => qm.Date)
                .Take(count)
                .ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get quiz metadata from Guardian RSS feed");
            return [];
        }
    }

    [XmlRoot("rss")]
    public class XmlRssRoot
    {
        [XmlElement("channel")] public XmlChannelElement Channel { get; init; } = new();
    }

    public class XmlChannelElement
    {
        [XmlElement("item")] public XmlItemElement[] Items { get; init; } = [];
    }

    public class XmlItemElement
    {
        [XmlElement("title")] public string Title { get; init; } = string.Empty;
        [XmlElement("pubDate")] public string PubDate { get; init; } = string.Empty;
        [XmlElement("link")] public string Link { get; init; } = string.Empty;

        public DateTime Date => DateTime.Parse(PubDate);
    }
}