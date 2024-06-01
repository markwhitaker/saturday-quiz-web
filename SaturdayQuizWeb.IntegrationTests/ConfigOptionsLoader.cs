using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.IntegrationTests.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.IntegrationTests;

public static class ConfigOptionsLoader
{
    private static IOptions<GuardianConfig>? _configOptions;

    public static IOptions<GuardianConfig> ConfigOptions => _configOptions ??= BuildConfigOptions();

    private static IOptions<GuardianConfig> BuildConfigOptions()
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
