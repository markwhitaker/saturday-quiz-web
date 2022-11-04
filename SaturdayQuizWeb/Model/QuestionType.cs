namespace SaturdayQuizWeb.Model;

[JsonConverter(typeof(StringEnumConverter))]
public enum QuestionType
{
    [EnumMember(Value = "NORMAL")]
    Normal,
    [EnumMember(Value = "WHAT_LINKS")]
    WhatLinks
}
