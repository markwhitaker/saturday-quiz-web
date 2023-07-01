var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import Scene from "./Scene.js";
import SceneType from "./SceneType.js";
import QuestionScore from "./QuestionScore.js";
export default class Presenter {
    constructor(quizRepository, scoreRepository) {
        this._quizRepository = quizRepository;
        this._scoreRepository = scoreRepository;
        this._sceneIndex = 0;
    }
    onViewReady(view) {
        return __awaiter(this, void 0, void 0, function* () {
            this._view = view;
            try {
                const quiz = yield this._quizRepository.loadLatestQuiz();
                this.onQuizLoaded(quiz);
            }
            catch (error) {
                console.log("Failed to load quiz. " + error.toString());
            }
        });
    }
    ;
    onNext() {
        if (this._sceneIndex < this._scenes.length - 1) {
            this._sceneIndex++;
            this.showScene();
        }
    }
    ;
    onPrevious() {
        if (this._sceneIndex > 0) {
            this._sceneIndex--;
            this.showScene();
        }
    }
    ;
    toggleScore() {
        const scene = this._scenes[this._sceneIndex];
        if (scene.type !== SceneType.QUESTION_ANSWER) {
            return;
        }
        const questionNumber = scene.question.number;
        let score = this._scoreRepository.getScore(questionNumber);
        switch (score) {
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
    shareScore() {
        return __awaiter(this, void 0, void 0, function* () {
            const totalScore = Presenter.formatTotalScore(this._scoreRepository.totalScore);
            const scoreBreakdown = this._scoreRepository.allScores
                .map((score, index) => score === QuestionScore.NONE ? null :
                score === QuestionScore.HALF ? (index + 1) + ' (half)' :
                    '' + (index + 1))
                .filter(scoreText => scoreText != null)
                .join(', ');
            try {
                yield navigator.share({
                    title: 'QUIZ RESULTS',
                    text: 'We have quizzed! Our total score this week is ' + totalScore + '...\n\n' + scoreBreakdown
                });
                console.log('Shared score');
            }
            catch (error) {
                console.log('Sharing score failed.', error);
            }
        });
    }
    onQuizLoaded(quiz) {
        this._scoreRepository.initialiseScores(quiz);
        this._scenes = Presenter.buildScenes(quiz, this._scoreRepository.hasScores);
        this.showScene();
        this._view.enableNavigation();
        this._view.onQuizLoaded();
    }
    ;
    showScene() {
        const scene = this._scenes[this._sceneIndex];
        const question = scene.question;
        const view = this._view;
        switch (scene.type) {
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
    }
    ;
    static buildScenes(quiz, jumpToAnswers) {
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
    static formatTotalScore(totalScore) {
        let totalScoreString = Math.floor(totalScore).toString();
        if (totalScore % 1 === QuestionScore.HALF) {
            totalScoreString += "½";
        }
        return totalScoreString;
    }
}
//# sourceMappingURL=Presenter.js.map