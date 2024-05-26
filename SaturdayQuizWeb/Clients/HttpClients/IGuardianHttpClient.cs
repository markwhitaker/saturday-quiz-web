namespace SaturdayQuizWeb.Clients.HttpClients;

public interface IGuardianHttpClient
{
    Task<string> GetStringAsync(string endpoint);
}
