import $ from "./jqueryModule.js";
import QuestionScore from "./QuestionScore.js";
import Presenter from "./Presenter.js";

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

const scoreClasses = Object.freeze(new Map<QuestionScore, string>([
    [QuestionScore.NONE, "none"],
    [QuestionScore.HALF, "half"],
    [QuestionScore.FULL, "full"]
]));

export default class View {
    #presenter: Presenter;

    constructor(presenter: Presenter) {
        this.#presenter = presenter;
    }

    onQuizLoaded = (): void => {
        elements.loaderContainer().fadeOut(() => {
            elements.quizContainer().removeClass('hidden');
        });
    };

    enableNavigation = (): void => {
        elements.navLeft().on("click", (e: JQuery.ClickEvent) => {
            this.#presenter.onPrevious();
            e.preventDefault();
        });
        elements.navRight().on("click", (e: JQuery.ClickEvent) => {
            this.#presenter.onNext();
            e.preventDefault();
        });
        elements.document().on("keyup", (e: JQuery.KeyUpEvent) => {
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

        elements.scoreTick().on("click", (e: JQuery.ClickEvent) => {
            this.#presenter.toggleScore();
            e.preventDefault();
        });

        elements.scoreShare().on("click", async (e: JQuery.ClickEvent) => {
            await this.#presenter.shareScore();
            e.preventDefault();
        });

        elements.skipToAnswers().on("click", (e: JQuery.ClickEvent) => {
            this.#presenter.toggleSkipToAnswers();
            e.preventDefault();
        });
    };

    showTitlePage = (): void => {
        elements.pageQuestion().hide();
        elements.pageTitle().show();
    };

    showQuestionPage = (): void => {
        elements.pageTitle().hide();
        elements.pageQuestion().show();
    };

    showQuestionsTitle = (dateString: string): void => {
        elements.title().text('Ready?');
        elements.quizDate().text(dateString);
        elements.totalScore().text('');
    };

    showAnswersTitle = (): void => {
        elements.title().text('Answers');
        elements.quizDate().text('');
        elements.totalScore().text('');
    };

    showEndTitle = (totalScore: string): void => {
        elements.title().text('End');
        elements.quizDate().text('');
        elements.totalScore().text("Total score: " + totalScore);
    };

    showQuestion = (question: { number: number, question: string, isWhatLinks: boolean }): void => {
        elements.questionNumber().text(question.number + '.');
        elements.question().text(question.question);
        elements.questionWhatLinks().toggleClass('visible', question.isWhatLinks);
    };

    hideAnswer = (): void => {
        elements.answer().hide();
    };

    showAnswer = (answer: string): void => {
        elements.answer().text(answer).show();
    };

    hideScoreTick = (): void => {
        elements.scoreTick().hide();
    };

    showScoreTick = (score: QuestionScore): void => {
        const className = scoreClasses.get(score);
        if (className) {
            elements.scoreTick()
                .removeClass(Array.from(scoreClasses.values()))
                .addClass(className)
                .show();
        }
    };

    hideScoreShare = (): void => {
        elements.scoreShare().hide();
    };

    showScoreShare = (): void => {
        elements.scoreShare().show();
    };

    hideSkipToAnswers = (): void => {
        elements.skipToAnswers().hide();
    };

    showSkipToAnswers = (): void => {
        elements.skipToAnswers().show();
    };

    setSkipToAnswers = (state: boolean): void => {
        elements.skipToAnswers().toggleClass('selected', state);
    };
}
