import Logger from "./Logger.js";
import NavigatorWrapper from "./NavigatorWrapper.js";
import QuestionScore from "./QuestionScore.js";
import QuizRepository from "./QuizRepository.js";
import Scene from "./Scene.js";
import ScoreRepository from "./ScoreRepository.js";
import Quiz from "./Quiz.js";
import View from "./View.js";

interface PresenterDependencies {
    navigatorWrapper?: NavigatorWrapper;
    quizRepository?: QuizRepository;
    scoreRepository?: ScoreRepository;
}

export default class Presenter {
    #navigatorWrapper: NavigatorWrapper;
    #quiz: Quiz;
    #quizRepository: QuizRepository;
    #sceneIndex: number;
    #scenes: Scene[];
    #scoreRepository: ScoreRepository;
    #skipToAnswers: boolean;
    #view!: View;

    constructor(dependencies: PresenterDependencies = {}) {
        this.#navigatorWrapper = dependencies.navigatorWrapper ?? new NavigatorWrapper();
        this.#quizRepository = dependencies.quizRepository ?? new QuizRepository();
        this.#scoreRepository = dependencies.scoreRepository ?? new ScoreRepository();
        this.#sceneIndex = 0;
        this.#quiz = {} as Quiz;
        this.#scenes = [];
        this.#skipToAnswers = false;
    }

    async onViewReady(view: View): Promise<void> {
        this.#view = view;

        try {
            this.#quiz = await this.#quizRepository.loadLatestQuiz();
            this.#onQuizLoaded();
        }
        catch (error) {
            Logger.log("Failed to load quiz. " + (error as Error).toString());
        }
    }

    onNext(): void {
        if (this.#sceneIndex < this.#scenes.length - 1) {
            this.#sceneIndex++;
            this.#showScene();
        }
    }

    onPrevious(): void {
        if (this.#sceneIndex > 0) {
            this.#sceneIndex--;
            this.#showScene();
        }
    }

    onSpace(): void {
        const scene = this.#scenes[this.#sceneIndex];
        if (!scene) return;
        
        switch (scene.type) {
            case Scene.Type.QUESTIONS_TITLE:
                this.toggleSkipToAnswers();
                break;
            case Scene.Type.QUESTION_ANSWER:
                this.toggleScore();
                break;
        }
    }

    toggleScore(): void {
        const scene = this.#scenes[this.#sceneIndex];
        if (!scene || scene.type !== Scene.Type.QUESTION_ANSWER) {
            return;
        }

        const questionNumber = scene.question!.number;
        let score = this.#scoreRepository.getScore(questionNumber);
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
        this.#scoreRepository.setScore(questionNumber, score);
        this.#view.showScoreTick(score);
    }

    toggleSkipToAnswers(): void {
        const scene = this.#scenes[this.#sceneIndex];
        if (!scene || scene.type !== Scene.Type.QUESTIONS_TITLE) {
            return;
        }

        this.#skipToAnswers = !this.#skipToAnswers;
        this.#view.setSkipToAnswers(this.#skipToAnswers);
        this.#buildScenes();
    }

    async shareScore(): Promise<void> {
        const totalScore = Presenter.#formatTotalScore(this.#scoreRepository.totalScore);
        const scoreBreakdown = this.#scoreRepository.allScores
            .map((score, index) =>
                score === QuestionScore.NONE ? null :
                score === QuestionScore.HALF ? (index + 1) + ' (half)' :
                '' + (index + 1)
            )
            .filter(scoreText => scoreText != null)
            .join(', ');

        try {
            await this.#navigatorWrapper.share({
                title: 'QUIZ RESULTS',
                text: 'We have quizzed! Our total score this week is ' + totalScore + '...\\n\\n' + scoreBreakdown
            });
        }
        catch (error) {
            // Ignore share errors
        }
    }

    #onQuizLoaded(): void {
        this.#scoreRepository.initialiseScores(this.#quiz);
        this.#skipToAnswers = this.#scoreRepository.hasScores;
        this.#buildScenes();
        this.#showScene();
        this.#view.enableNavigation();
        this.#view.setSkipToAnswers(this.#skipToAnswers);
        this.#view.onQuizLoaded();
    }

    #showScene(): void {
        const scene = this.#scenes[this.#sceneIndex];
        if (!scene) return;
        
        const question = scene.question;
        const view = this.#view;

        switch (scene.type) {
            case Scene.Type.QUESTIONS_TITLE:
                view.hideScoreTick();
                view.hideScoreShare();
                view.showQuestionsTitle(scene.date!.toDisplayString());
                view.showTitlePage();
                view.showSkipToAnswers();
                break;
            case Scene.Type.QUESTION:
                view.hideScoreTick();
                view.hideScoreShare();
                view.hideSkipToAnswers();
                view.showQuestion(question!);
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
                view.showQuestion(question!);
                view.showAnswer(question!.answer);
                view.showQuestionPage();
                view.showScoreTick(this.#scoreRepository.getScore(question!.number));
                break;
            case Scene.Type.END_TITLE:
                view.hideScoreTick();
                view.hideSkipToAnswers();
                view.showEndTitle(Presenter.#formatTotalScore(this.#scoreRepository.totalScore));
                view.showTitlePage();
                if (this.#navigatorWrapper.isShareSupported) {
                    view.showScoreShare();
                }
                break;
        }
    }

    #buildScenes(): void {
        this.#scenes = [];

        this.#scenes.push(Scene.questionsTitleScene(this.#quiz.date));

        if (!this.#skipToAnswers) {
            // First just show the questions
            for (const question of this.#quiz.questions) {
                this.#scenes.push(Scene.questionScene(question));
            }
            this.#scenes.push(Scene.answersTitleScene());
        }

        // Now recap the questions, showing the answer after each one
        for (const question of this.#quiz.questions) {
            this.#scenes.push(Scene.questionScene(question));
            this.#scenes.push(Scene.questionAnswerScene(question));
        }

        this.#scenes.push(Scene.endTitleScene());
    }

    static #formatTotalScore(totalScore: number): string {
        let totalScoreString = Math.floor(totalScore).toString();
        if (totalScore % 1 === QuestionScore.HALF) {
            totalScoreString += "½";
        }
        return totalScoreString;
    }
}
