namespace SaturdayQuizWeb.Model;

public record QuestionModel
{
    public int Number { get; init; }
    public QuestionType Type { get; init; }
    public string Question { get; init; }
    public string Answer { get; set; }
}