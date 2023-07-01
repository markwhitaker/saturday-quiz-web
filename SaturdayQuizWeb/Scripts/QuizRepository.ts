import Quiz from "./Quiz.js";

export default class QuizRepository {
    private static readonly QUIZ_ENDPOINT = "api/quiz";

    async loadLatestQuiz() : Promise<Quiz> {
        const response = await fetch(QuizRepository.QUIZ_ENDPOINT);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${(QuizRepository.QUIZ_ENDPOINT)}. ${response.status}: ${response.statusText}`);
        }
        const quizJson = await response.json();
        return new Quiz(quizJson);
    }
}
