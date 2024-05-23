using Microsoft.Extensions.Logging.Testing;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.IntegrationTests.Clients;

[TestFixture]
public class GuardianApiClientTests
{
    private IGuardianApiClient _guardianApiClient = null!;

    [SetUp]
    public void SetUp()
    {
        _guardianApiClient = new GuardianApiClient(
            new HttpClient(),
            ConfigOptionsLoader.ConfigOptions,
            new FakeLogger<GuardianApiClient>());
    }

    [TestCase(1)]
    [TestCase(10)]
    public async Task GivenClient_WhenQuizzesAreRequested_ThenExpectedMetadataIsReturnedInExpectedOrder(
        int expectedCount)
    {
        // Given

        // When
        var metadata = await _guardianApiClient.GetQuizMetadataAsync(expectedCount);

        // Then
        Assert.That(metadata, Has.Exactly(expectedCount).Items);
        Assert.That(metadata, Is.Ordered.Descending.By(nameof(QuizMetadata.Date)));
        Assert.That(metadata, Is.All.Matches<QuizMetadata>(qm => qm.Source == "API"));
    }
}
