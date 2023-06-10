import Quiz from "./Quiz.js";
import QuizMetadata from "../js/QuizMetadata.js";

export default class QuizRepository {
    #quizEndpoint = Object.freeze("api/quiz");

    async loadLatestQuiz() {
        const response = await fetch(this.#quizEndpoint);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${this.#quizEndpoint}. ${response.status}: ${response.statusText}`);
        }
        const quizJson = await response.json();
        return new Quiz(quizJson);
    }
}
