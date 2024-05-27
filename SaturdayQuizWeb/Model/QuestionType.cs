using System.Text.Json.Serialization;

namespace SaturdayQuizWeb.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum QuestionType
{
    NORMAL,
    WHAT_LINKS
}
