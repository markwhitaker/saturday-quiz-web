"use strict";

const apiEndpoint = "api/quiz";

class QuizRepository {
    loadLatestQuiz() {
        return fetch(apiEndpoint)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to fetch " + apiEndpoint + ". " + response.status + ": " + response.statusText);
                }
                return response.json();
            })
            .then(quizJson => new Quiz(quizJson));
    }
}
