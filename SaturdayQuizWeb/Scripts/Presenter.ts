import Scene from "./Scene.js";
import SceneType from "./SceneType.js";
import QuestionScore from "./QuestionScore.js";
import QuizRepository from "./QuizRepository.js";
import ScoreRepository from "./ScoreRepository.js";
import View from "./View.js";

export default class Presenter {
    private readonly _quizRepository: QuizRepository;
    private readonly _scoreRepository: ScoreRepository;
    private _view: View;
    private _scenes: Scene[];
    private _sceneIndex: number;

    constructor(
        quizRepository: QuizRepository,
        scoreRepository: ScoreRepository) {
        this._quizRepository = quizRepository;
        this._scoreRepository = scoreRepository;
        this._sceneIndex = 0;
    }

    async onViewReady(view) {
        this._view = view;

        try {
            const quiz = await this._quizRepository.loadLatestQuiz();
            this.onQuizLoaded(quiz);
        }
        catch (error) {
            console.log("Failed to load quiz. " + error.toString());
        }
    };

    onNext() {
        if (this._sceneIndex < this._scenes.length - 1) {
            this._sceneIndex++;
            this.showScene();
        }
    };

    onPrevious() {
        if (this._sceneIndex > 0) {
            this._sceneIndex--;
            this.showScene();
        }
    };

    toggleScore() {
        const scene = this._scenes[this._sceneIndex];
        if (scene.type !== SceneType.QUESTION_ANSWER) {
            return;
        }

        const questionNumber = scene.question.number;
        let score = this._scoreRepository.getScore(questionNumber);
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
        this._scoreRepository.setScore(questionNumber, score);
        this._view.showScoreTick(score);
    }

    async shareScore() {
        const totalScore = Presenter.formatTotalScore(this._scoreRepository.totalScore);
        const scoreBreakdown = this._scoreRepository.allScores
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

    private onQuizLoaded(quiz) {
        this._scoreRepository.initialiseScores(quiz);
        this._scenes = Presenter.buildScenes(quiz, this._scoreRepository.hasScores);
        this.showScene();
        this._view.enableNavigation();
        this._view.onQuizLoaded();
    };

    private showScene() {
        const scene = this._scenes[this._sceneIndex];
        const question = scene.question;
        const view = this._view;

        switch(scene.type) {
            case SceneType.QUESTIONS_TITLE:
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
            case SceneType.QUESTION:
                view.hideScoreTick();
                view.hideScoreShare();
                view.showQuestion(question);
                view.hideAnswer();
                view.showQuestionPage();
                break;
            case SceneType.ANSWERS_TITLE:
                view.hideScoreTick();
                view.hideScoreShare();
                view.showTitlePage();
                view.showAnswersTitle();
                break;
            case SceneType.QUESTION_ANSWER:
                view.hideScoreShare();
                view.showQuestion(question);
                view.showAnswer(question.answer);
                view.showQuestionPage();
                view.showScoreTick(this._scoreRepository.getScore(question.number));
                break;
            case SceneType.END_TITLE:
                view.hideScoreTick();
                view.showEndTitle(Presenter.formatTotalScore(this._scoreRepository.totalScore));
                view.showTitlePage();
                view.showScoreShare();
                break;
        }
    };

    private static buildScenes(quiz, jumpToAnswers) {
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

    private static formatTotalScore(totalScore) {
        let totalScoreString = Math.floor(totalScore).toString();
        if (totalScore % 1 === QuestionScore.HALF) {
            totalScoreString += "½"
        }
        return totalScoreString;
    }
}
