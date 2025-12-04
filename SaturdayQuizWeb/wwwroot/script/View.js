import $ from "./jqueryModule.js";
import QuestionScore from "./QuestionScore.js";

const elements = Object.freeze({
    answer: () => $("#answer"),
    document: () => $(document),
    loaderContainer: () => $("#loader-container"),
    navLeft: () => $("#nav-left"),
    navRight: () => $("#nav-right"),
    pageQuestion: () => $("#page-question"),
    pageTitle: () => $("#page-title"),
    question: () => $("#question"),
    questionNumber: () => $("#question-number"),
    questionWhatLinks: () => $("#question-what-links"),
    quizContainer: () => $("#quiz-container"),
    quizDate: () => $("#quiz-date"),
    scoreShare: () => $("#score-share"),
    scoreTick: () => $("#score-tick"),
    skipToAnswers: () => $("#skip-to-answers"),
    title: () => $("#title"),
    totalScore: () => $("#total-score")
});

const scoreClasses = Object.freeze(new Map([
    [QuestionScore.NONE, "none"],
    [QuestionScore.HALF, "half"],
    [QuestionScore.FULL, "full"]
]));

export default class View {
    #presenter;

    constructor(presenter) {
        this.#presenter = presenter;
    };

    onQuizLoaded = () =>
        elements.loaderContainer().fadeOut(() => {
            elements.quizContainer().removeClass('hidden');
        });

    enableNavigation = () => {
        elements.navLeft().on("click", e => {
            this.#presenter.onPrevious();
            e.preventDefault();
        });
        elements.navRight().on("click", e => {
            this.#presenter.onNext();
            e.preventDefault();
        });
        elements.document().on("keyup", e => {
            switch (e.code) {
                case 'ArrowLeft':
                    this.#presenter.onPrevious();
                    break;
                case 'ArrowRight':
                    this.#presenter.onNext();
                    break;
                case 'Space':
                    this.#presenter.onSpace();
                    break;
                default:
                    return;
            }
            e.preventDefault();
        });

        elements.scoreTick().on("click", e => {
            this.#presenter.toggleScore();
            e.preventDefault();
        });

        elements.scoreShare().on("click", async e => {
            await this.#presenter.shareScore();
            e.preventDefault();
        });

        elements.skipToAnswers().on("click", e => {
            this.#presenter.toggleSkipToAnswers();
            e.preventDefault();
        });
    };

    showTitlePage = () => {
        elements.pageQuestion().hide();
        elements.pageTitle().show();
    };

    showQuestionPage = () => {
        elements.pageTitle().hide();
        elements.pageQuestion().show();
    };

    showQuestionsTitle = dateString => {
        elements.title().text('Ready?');
        elements.quizDate().text(dateString);
        elements.totalScore().text('');
    };

    showAnswersTitle = () => {
        elements.title().text('Answers');
        elements.quizDate().text('');
        elements.totalScore().text('');
    };

    showEndTitle = totalScore => {
        elements.title().text('End');
        elements.quizDate().text('');
        elements.totalScore().text("Total score: " + totalScore);
    };

    showQuestion = question => {
        elements.questionNumber().text(question.getNumber() + '.');
        elements.question().text(question.getQuestion());
        elements.questionWhatLinks().toggleClass('visible', question.isWhatLinks());
    };

    hideAnswer = () => elements.answer().hide();

    showAnswer = answer => elements.answer().text(answer).show();

    hideScoreTick = () => elements.scoreTick().hide();

    showScoreTick = score =>
        elements.scoreTick()
            .removeClass(Array.from(scoreClasses.values()))
            .addClass(scoreClasses.get(score))
            .show();

    hideScoreShare = () => elements.scoreShare().hide();

    showScoreShare = () => elements.scoreShare().show();

    hideSkipToAnswers = () => elements.skipToAnswers().hide();

    showSkipToAnswers = () => elements.skipToAnswers().show();

    setSkipToAnswers = state => elements.skipToAnswers().toggleClass('selected', state);
}
