namespace SaturdayQuizWeb.Model
{
    public class QuestionModel
    {
        public int Number { get; set; }
        public QuestionType Type { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
