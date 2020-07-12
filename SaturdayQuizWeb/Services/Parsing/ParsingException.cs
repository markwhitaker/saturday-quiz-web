using System;

namespace SaturdayQuizWeb.Services.Parsing
{
    public class ParsingException : Exception
    {
        public ParsingException(string message) : base(message)
        {
        }
    }
}