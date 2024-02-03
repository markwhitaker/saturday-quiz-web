import Scene from "./Scene.js";
import QuestionScore from "./QuestionScore.js";

export default class Presenter {
    constructor(quizRepository, scoreRepository) {
        this.quizRepository = quizRepository;
        this.scoreRepository = scoreRepository;
        this.sceneIndex = 0;
    }

    async onViewReady(view) {
        this.view = view;

        try {
            const quiz = await this.quizRepository.loadLatestQuiz();
            this.#onQuizLoaded(quiz);
        }
        catch (error) {
            console.log("Failed to load quiz. " + error.toString());
        }
    };

    onNext() {
        if (this.sceneIndex < this.scenes.length - 1) {
            this.sceneIndex++;
            this.#showScene();
        }
    };

    onPrevious() {
        if (this.sceneIndex > 0) {
            this.sceneIndex--;
            this.#showScene();
        }
    };

    toggleScore() {
        const scene = this.scenes[this.sceneIndex];
        if (scene.type !== Scene.Type.QUESTION_ANSWER) {
            return;
        }

        const questionNumber = scene.question.number;
        let score = this.scoreRepository.getScore(questionNumber);
        switch (score)
        {
            case QuestionScore.NONE:
                score = QuestionScore.FULL;
                break;
            case QuestionScore.FULL:
                score = QuestionScore.HALF;
                break;
            case QuestionScore.HALF:
                score = QuestionScore.NONE;
                break;
        }
        this.scoreRepository.setScore(questionNumber, score);
        this.view.showScoreTick(score);
    }

    async shareScore() {
        const totalScore = Presenter.#formatTotalScore(this.scoreRepository.totalScore);
        const scoreBreakdown = this.scoreRepository.allScores
            .map((score, index) =>
                score === QuestionScore.NONE ? null :
                score === QuestionScore.HALF ? (index + 1) + ' (half)' :
                '' + (index + 1)
            )
            .filter(scoreText => scoreText != null)
            .join(', ');

        try {
            await navigator.share({
                title: 'QUIZ RESULTS',
                text: 'We have quizzed! Our total score this week is ' + totalScore + '...\n\n' + scoreBreakdown
            });
            console.log('Shared score');
        }
        catch (error) {
            console.log('Sharing score failed.', error);
        }
    }

    #onQuizLoaded(quiz) {
        this.scoreRepository.initialiseScores(quiz);
        this.scenes = Presenter.#buildScenes(quiz, this.scoreRepository.hasScores);
        this.#showScene();
        this.view.enableNavigation();
        this.view.onQuizLoaded();
    };

    #showScene() {
        const scene = this.scenes[this.sceneIndex];
        const question = scene.question;
        const view = this.view;

        switch(scene.type) {
            case Scene.Type.QUESTIONS_TITLE:
                const dateString = scene.date.toLocaleDateString("en-GB", {
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                });
                view.hideScoreTick();
                view.hideScoreShare();
                view.showQuestionsTitle(dateString);
                view.showTitlePage();
                break;
            case Scene.Type.QUESTION:
                view.hideScoreTick();
                view.hideScoreShare();
                view.showQuestion(question);
                view.hideAnswer();
                view.showQuestionPage();
                break;
            case Scene.Type.ANSWERS_TITLE:
                view.hideScoreTick();
                view.hideScoreShare();
                view.showTitlePage();
                view.showAnswersTitle();
                break;
            case Scene.Type.QUESTION_ANSWER:
                view.hideScoreShare();
                view.showQuestion(question);
                view.showAnswer(question.answer);
                view.showQuestionPage();
                view.showScoreTick(this.scoreRepository.getScore(question.number));
                break;
            case Scene.Type.END_TITLE:
                view.hideScoreTick();
                view.showEndTitle(Presenter.#formatTotalScore(this.scoreRepository.totalScore));
                view.showTitlePage();
                view.showScoreShare();
                break;
        }
    };

    static #buildScenes(quiz, jumpToAnswers) {
        const scenes = [];

        scenes.push(Scene.questionsTitleScene(quiz.date));

        if (!jumpToAnswers) {
            // First just show the questions
            for (const question of quiz.questions) {
                scenes.push(Scene.questionScene(question));
            }
        }

        scenes.push(Scene.answersTitleScene());

        // Now recap the questions, showing the answer after each one
        for (const question of quiz.questions) {
            scenes.push(Scene.questionScene(question));
            scenes.push(Scene.questionAnswerScene(question));
        }

        scenes.push(Scene.endTitleScene());

        return scenes;
    }

    static #formatTotalScore(totalScore) {
        let totalScoreString = Math.floor(totalScore);
        if (totalScore % 1 === QuestionScore.HALF) {
            totalScoreString += "½"
        }
        return totalScoreString;
    }
}
