using System;
using Microsoft.Extensions.Configuration;

namespace SaturdayQuizWeb.Utils
{
    public interface IConfigVariables
    {
        string GuardianApiKey { get; }
    }

    public class ConfigVariables : IConfigVariables
    {
        private readonly IConfiguration _configuration;

        public ConfigVariables(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GuardianApiKey => GetValue("GuardianApiKey");

        private string GetValue(string key) =>
            _configuration[key] ?? throw new Exception($"Config value {key} not found");
    }
}
