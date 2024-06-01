using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.IntegrationTests.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.IntegrationTests;

public static class ConfigOptionsLoader
{
    public static IOptions<GuardianConfig> ConfigOptions
    {
        get
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .AddUserSecrets<QuizMetadataServiceTests>()
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection()
                .Configure<GuardianConfig>(configuration.GetSection(Constants.ConfigSectionGuardian))
                .BuildServiceProvider();

            var configOptions = services.GetService<IOptions<GuardianConfig>>() ?? throw new Exception(
                $"Failed to get IOptions<{nameof(GuardianConfig)}> from service provider");

            return configOptions;
        }
    }
}
