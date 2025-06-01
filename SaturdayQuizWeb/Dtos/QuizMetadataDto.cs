using JetBrains.Annotations;
using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Dtos;

public sealed record QuizMetadataDto
{
    public string Id { [UsedImplicitly] get; }
    public DateTime Date { [UsedImplicitly] get; }
    public string Title { [UsedImplicitly] get; }
    public string Url { [UsedImplicitly] get; }
    public string Source { [UsedImplicitly] get; }

    public QuizMetadataDto(QuizMetadata quizMetadata)
    {
        Id = quizMetadata.Id;
        Date = quizMetadata.Date;
        Title = quizMetadata.Title;
        Url = quizMetadata.Url;
        Source = quizMetadata.Source;
    }
}
