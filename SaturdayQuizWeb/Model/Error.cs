namespace SaturdayQuizWeb.Model;

public sealed record Error
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string ErrorMessage { get; }

    public Error(Exception e)
    {
        ErrorMessage = e.Message;
    }
}
