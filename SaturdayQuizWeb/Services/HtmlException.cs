using System;

namespace SaturdayQuizWeb.Services
{
    public class HtmlException : Exception
    {
        public HtmlException(string message) : base(message)
        {
        }
    }
}