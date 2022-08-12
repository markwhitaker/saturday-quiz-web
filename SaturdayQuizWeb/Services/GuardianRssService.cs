using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Services;

public interface IGuardianRssService
{
    Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count);
}

public class GuardianRssService : IGuardianRssService
{
    private readonly GuardianConfig _guardianConfig;
    private readonly IGuardianWebsiteService _guardianWebsiteService;

    public GuardianRssService(IOptions<GuardianConfig> guardianConfig, IGuardianWebsiteService guardianWebsiteService)
    {
        _guardianConfig = guardianConfig.Value;
        _guardianWebsiteService = guardianWebsiteService;
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        try
        {
            var contents = await _guardianWebsiteService.GetPageContentAsync(_guardianConfig.RssEndpoint);
            var xmlDoc = XDocument.Parse(contents);
            return xmlDoc.Element("rss")!
                .Element("channel")!
                .Elements("item")
                .Take(count)
                .Select(item => new QuizMetadata
                {
                    Title = item.Element("title")?.Value ?? throw new InvalidOperationException(),
                    Date = DateTime.Parse(item.Element("pubDate")?.Value ?? throw new InvalidOperationException()),
                    Url = item.Element("link")?.Value ?? throw new InvalidOperationException(),
                    Id = item.Element("link")?.Value.Replace(_guardianConfig.WebsiteBaseUrl, "") ?? throw new InvalidOperationException(),
                })
                .ToList();
        }
        catch (Exception)
        {
            return new List<QuizMetadata>();
        }
    }
}
