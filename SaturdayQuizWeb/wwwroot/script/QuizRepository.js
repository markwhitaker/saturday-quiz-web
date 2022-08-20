import Quiz from "./Quiz.js";

export default class QuizRepository {
    #apiEndpoint = Object.freeze("api/quiz");

    async loadLatestQuiz() {
        const response = await fetch(this.#apiEndpoint);
        if (!response.ok) {
            throw new Error("Failed to fetch " + this.#apiEndpoint + ". " + response.status + ": " + response.statusText);
        }
        const quizJson = await response.json();
        return new Quiz(quizJson);
    }
}
