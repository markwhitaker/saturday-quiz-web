using System.Collections.Generic;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Services
{
    public interface IHtmlService
    {
        IEnumerable<Question> FindQuestions(string html);
    }
}