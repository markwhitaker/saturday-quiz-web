class QuizRepository {
    loadLatestQuiz(onQuizLoaded) {
        $.get("api/quiz", function (quizObject) {
            onQuizLoaded(new Quiz(quizObject));
        });
    }
}
