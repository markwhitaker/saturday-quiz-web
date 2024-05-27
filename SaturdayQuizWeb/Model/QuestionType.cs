using System.Text.Json.Serialization;

namespace SaturdayQuizWeb.Model;

[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
//[JsonConverter(typeof(StringEnumConverter))]
public enum QuestionType
{
    [EnumMember(Value = "NORMAL")]
    Normal,
    [EnumMember(Value = "WHAT_LINKS")]
    WhatLinks
}
