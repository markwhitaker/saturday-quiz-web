using JetBrains.Annotations;
using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Dtos;

public record QuestionDto
{
    public int Number { [UsedImplicitly] get; }
    public string Type { [UsedImplicitly] get; }
    public string Question { [UsedImplicitly] get; }
    public string[] WhatLinks { [UsedImplicitly] get; }
    public string Answer { [UsedImplicitly] get; }

    public QuestionDto(QuestionModel questionModel)
    {
        Number = questionModel.Number;
        Type = questionModel.Type == QuestionType.Normal ? "NORMAL" : "WHAT_LINKS";
        Question = questionModel.Question;
        WhatLinks = questionModel.WhatLinks.ToArray();
        Answer = questionModel.Answer;
    }
}
