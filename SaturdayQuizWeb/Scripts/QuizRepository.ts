import Quiz from "./Quiz.js";

export default class QuizRepository {
    private static readonly QUIZ_ENDPOINT = "api/quiz";

    async loadLatestQuiz() {
        const response = await fetch(QuizRepository.QUIZ_ENDPOINT);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${(QuizRepository.QUIZ_ENDPOINT)}. ${response.status}: ${response.statusText}`);
        }
        const quizJson = await response.json();
        return new Quiz(quizJson);
    }
}
