using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.IntegrationTests.Clients;

[TestFixture]
public class GuardianRssClientTests
{
    private IGuardianRssClient _guardianRssClient = null!;

    [SetUp]
    public void SetUp()
    {
        var configOptions = ConfigOptionsLoader.ConfigOptions;
        var guardianWebsiteClient = new GuardianWebsiteClient(new HttpClient(), configOptions);
        _guardianRssClient = new GuardianRssClient(configOptions, guardianWebsiteClient);
    }

    [TestCase(1)]
    [TestCase(10)]
    public async Task GivenClient_WhenQuizzesAreRequested_ThenExpectedMetadataIsReturnedInExpectedOrder(
        int expectedCount)
    {
        // Given

        // When
        var metadata = await _guardianRssClient.GetQuizMetadataAsync(expectedCount);

        // Then
        Assert.That(metadata, Has.Exactly(expectedCount).Items);
        Assert.That(metadata, Is.Ordered.Descending.By("Date"));
        Assert.That(metadata, Is.All.Matches<QuizMetadata>(qm => qm.Source == "RSS"));
    }
}
