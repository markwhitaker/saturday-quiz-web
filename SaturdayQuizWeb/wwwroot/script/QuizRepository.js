import Quiz from "./Quiz.js";
import QuizMetadata from "./QuizMetadata.js";

export default class QuizRepository {
    #quizEndpoint = Object.freeze("api/quiz?id=");
    #quizMetadataEndpoint = Object.freeze("api/quiz-metadata?count=1");

    async loadLatestQuiz() {
        const metadata = await this.#loadQuizMetadata();
        const apiEndpoint = this.#quizEndpoint + metadata.id;
        const response = await fetch(apiEndpoint);
        if (!response.ok) {
            this.#error(apiEndpoint, response)
        }
        const quizJson = await response.json();
        return new Quiz(quizJson);
    }

    async #loadQuizMetadata() {
        const response = await fetch(this.#quizMetadataEndpoint);
        if (!response.ok) {
            this.#error(this.#quizMetadataEndpoint, response)
        }
        const quizMetadataJson = await response.json();
        return new QuizMetadata(quizMetadataJson[0]);
    }

    #error(endpoint, response) {
        throw new Error("Failed to fetch " + endpoint + ". " + response.status + ": " + response.statusText);
    }
}
