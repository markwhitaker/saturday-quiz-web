import Scene from "./Scene.js";
import QuestionScore from "./QuestionScore.js";

export default class Presenter {
    constructor(quizRepository, scoreRepository) {
        this.quizRepository = quizRepository;
        this.scoreRepository = scoreRepository;
        this.sceneIndex = 0;
        this.quiz = {};
        this.scenes = [];
        this.skipToAnswers = false;
    }

    async onViewReady(view) {
        this.view = view;

        try {
            this.quiz = await this.quizRepository.loadLatestQuiz();
            this.#onQuizLoaded();
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

    toggleSkipToAnswers() {
        this.skipToAnswers = !this.skipToAnswers;
        this.view.setSkipToAnswers(this.skipToAnswers);
        this.#buildScenes();
    }

    #onQuizLoaded() {
        this.scoreRepository.initialiseScores(this.quiz);
        this.skipToAnswers = this.scoreRepository.hasScores;
        this.#buildScenes();
        this.#showScene();
        this.view.enableNavigation();
        this.view.setSkipToAnswers(this.skipToAnswers);
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
                view.showSkipToAnswers();
                break;
            case Scene.Type.QUESTION:
                view.hideScoreTick();
                view.hideScoreShare();
                view.hideSkipToAnswers();
                view.showQuestion(question);
                view.hideAnswer();
                view.showQuestionPage();
                break;
            case Scene.Type.ANSWERS_TITLE:
                view.hideScoreTick();
                view.hideScoreShare();
                view.hideSkipToAnswers();
                view.showTitlePage();
                view.showAnswersTitle();
                break;
            case Scene.Type.QUESTION_ANSWER:
                view.hideScoreShare();
                view.hideSkipToAnswers();
                view.showQuestion(question);
                view.showAnswer(question.answer);
                view.showQuestionPage();
                view.showScoreTick(this.scoreRepository.getScore(question.number));
                break;
            case Scene.Type.END_TITLE:
                view.hideScoreTick();
                view.hideSkipToAnswers();
                view.showEndTitle(Presenter.#formatTotalScore(this.scoreRepository.totalScore));
                view.showTitlePage();
                view.showScoreShare();
                break;
        }
    };

    #buildScenes() {
        this.scenes = [];

        this.scenes.push(Scene.questionsTitleScene(this.quiz.date));

        if (!this.skipToAnswers) {
            // First just show the questions
            for (const question of this.quiz.questions) {
                this.scenes.push(Scene.questionScene(question));
            }
        }

        this.scenes.push(Scene.answersTitleScene());

        // Now recap the questions, showing the answer after each one
        for (const question of this.quiz.questions) {
            this.scenes.push(Scene.questionScene(question));
            this.scenes.push(Scene.questionAnswerScene(question));
        }

        this.scenes.push(Scene.endTitleScene());
    }

    static #formatTotalScore(totalScore) {
        let totalScoreString = Math.floor(totalScore);
        if (totalScore % 1 === QuestionScore.HALF) {
            totalScoreString += "½"
        }
        return totalScoreString;
    }
}
