using SaturdayQuizWeb.Extensions;

namespace SaturdayQuizWeb.Models;

public sealed record QuizMetadata
{
    private readonly DateTime _date;

    public required string Id { get; init; } = string.Empty;

    public required DateTime Date
    {
        get => _date;
        init => _date = value.ToDateUtc();
    }

    public string Title { get; init; } = string.Empty;

    public string Url { get; init; } = string.Empty;

    public string Source { get; init; } = string.Empty;

    public override int GetHashCode() => Url.GetHashCode();

    public bool Equals(QuizMetadata? other) => Url.Equals(other?.Url);
}
