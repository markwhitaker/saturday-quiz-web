using System.Collections.Immutable;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianRssClient : IGuardianQuizMetadataClient;

public class GuardianRssClient : IGuardianRssClient
{
    private readonly GuardianConfig _guardianConfig;
    private readonly IGuardianWebsiteClient _guardianWebsiteClient;

    public GuardianRssClient(IOptions<GuardianConfig> guardianConfig, IGuardianWebsiteClient guardianWebsiteClient)
    {
        _guardianConfig = guardianConfig.Value;
        _guardianWebsiteClient = guardianWebsiteClient;
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        try
        {
            var contents = await _guardianWebsiteClient.GetPageContentAsync(_guardianConfig.RssEndpoint);

            var xmlSerializer = new XmlSerializer(typeof(XmlRssRoot));
            using var reader = new StringReader(contents);
            var xmlRss = xmlSerializer.Deserialize(reader) as XmlRssRoot;
            return xmlRss!.Channel.Items
                .Select(item => new QuizMetadata
                {
                    Title = item.Title.Trim(),
                    Url = item.Link.Trim(),
                    Date = item.Date,
                    Id = item.Link.Trim().Replace(_guardianConfig.WebsiteBaseUrl, string.Empty),
                    Source = Constants.SourceRss
                })
                .Distinct()
                .OrderByDescending(qm => qm.Date)
                .Take(count)
                .ToList();
        }
        catch (Exception)
        {
            return Array.Empty<QuizMetadata>();
        }
    }

    [XmlRoot("rss")]
    public class XmlRssRoot
    {
        [XmlElement("channel")]
        public XmlChannelElement Channel { get; init; } = new();
    }

    public class XmlChannelElement
    {
        [XmlElement("item")]
        public XmlItemElement[] Items { get; init; } = Array.Empty<XmlItemElement>();
    }

    public class XmlItemElement
    {
        [XmlElement("title")]
        public string Title { get; init; } = string.Empty;
        [XmlElement("pubDate")]
        public string PubDate { get; init; } = string.Empty;
        [XmlElement("link")]
        public string Link { get; init; } = string.Empty;

        public DateTime Date => DateTime.Parse(PubDate);
    }
}
