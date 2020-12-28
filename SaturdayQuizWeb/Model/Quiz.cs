using System;
using System.Collections.Generic;

namespace SaturdayQuizWeb.Model
{
    public class Quiz
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public IEnumerable<QuestionModel> Questions { get; set; }
    }
}