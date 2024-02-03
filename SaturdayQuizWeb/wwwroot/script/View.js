import $ from "./jqueryModule.js";
import QuestionScore from "./QuestionScore.js";

const elements = Object.freeze({
    answer: () => $("#answer"),
    box: () => $("#box"),
    document: () => $(document),
    loaderBox: () => $("#loader-box"),
    navLeft: () => $("#nav-left"),
    navRight: () => $("#nav-right"),
    pageQuestion: () => $("#page-question"),
    pageTitle: () => $("#page-title"),
    question: () => $("#question"),
    questionNumber: () => $("#question-number"),
    questionWhatLinks: () => $("#question-what-links"),
    quizDate: () => $("#quiz-date"),
    scoreShare: () => $("#score-share"),
    scoreTick: () => $("#score-tick"),
    title: () => $("#title"),
    totalScore: () => $("#total-score")
});

export default class View {
    constructor(presenter) {
        this.presenter = presenter;
    };

    onQuizLoaded = () =>
        elements.loaderBox().fadeOut(function () {
            elements.box().removeClass('hidden');
        });

    enableNavigation = () => {
        elements.navLeft().click(e => {
            this.presenter.onPrevious();
            e.preventDefault();
        });
        elements.navRight().click(e => {
            this.presenter.onNext();
            e.preventDefault();
        });
        elements.document().keyup(e => {
            switch (e.code) {
                case 'ArrowLeft':
                    this.presenter.onPrevious();
                    break;
                case 'ArrowRight':
                    this.presenter.onNext();
                    break;
                case 'Space':
                    this.presenter.toggleScore();
                    break;
                default:
                    return;
            }
            e.preventDefault();
        });

        elements.scoreTick().click(e => {
            this.presenter.toggleScore();
            e.preventDefault();
        })

        elements.scoreShare().click(async e => {
            await this.presenter.shareScore();
            e.preventDefault();
        })
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
        elements.questionNumber().text(question.number + '.');
        elements.question().text(question.question);
        elements.questionWhatLinks().toggleClass('visible', question.isWhatLinks);
    };

    hideAnswer = () => elements.answer().hide();

    showAnswer = answer => elements.answer().text(answer).show();

    hideScoreTick = () => elements.scoreTick().hide();

    showScoreTick = score => {
        let scoreClass = "";
        switch (score)
        {
            case QuestionScore.NONE:
                scoreClass = "none";
                break;
            case QuestionScore.HALF:
                scoreClass = "half";
                break;
            case QuestionScore.FULL:
                scoreClass = "full";
        }

        elements.scoreTick()
            .removeClass()
            .addClass(scoreClass)
            .show();
    };

    hideScoreShare = () => elements.scoreShare().hide();

    showScoreShare = () => elements.scoreShare().show();
}
