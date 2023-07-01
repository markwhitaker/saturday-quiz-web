import $ from "./jqueryModule.js";
import QuestionScore from "./QuestionScore.js";
import Presenter from "./Presenter.js";

export default class View {
    private readonly _presenter: Presenter;

    constructor(presenter: Presenter) {
        this._presenter = presenter;
    };

    onQuizLoaded() {
        $('#loader-box').fadeOut(function(){
            $('#box').removeClass('hidden');
        });
    }

    enableNavigation() {
        $('#nav-left').click(e => {
            this._presenter.onPrevious();
            e.preventDefault();
        });
        $('#nav-right').click(e => {
            this._presenter.onNext();
            e.preventDefault();
        });
        $(document).keyup(e => {
            switch (e.code) {
                case 'ArrowLeft':
                    this._presenter.onPrevious();
                    break;
                case 'ArrowRight':
                    this._presenter.onNext();
                    break;
                case 'Space':
                    this._presenter.toggleScore();
                    break;
                default:
                    return;
            }
            e.preventDefault();
        });

        $('#score-tick').click(e => {
            this._presenter.toggleScore();
            e.preventDefault();
        })

        $('#score-share').click(async e => {
            await this._presenter.shareScore();
            e.preventDefault();
        })
    };

    showTitlePage() {
        $('#page-question').hide();
        $('#page-title').show();
    };

    showQuestionPage() {
        $('#page-title').hide();
        $('#page-question').show();
    };

    showQuestionsTitle(dateString) {
        $('#title').text('Ready?');
        $('#quiz-date').text(dateString);
        $('#total-score').text('');
    };

    showAnswersTitle() {
        $('#title').text('Answers');
        $('#quiz-date').text('');
        $('#total-score').text('');
    };

    showEndTitle(totalScore) {
        $('#title').text('End');
        $('#quiz-date').text('');
        $('#total-score').text("Total score: " + totalScore);
    };

    showQuestion(question) {
        $('#question-number').text(question.number + '.');
        $('#question').text(question.question);
        $('#question-what-links').toggleClass('visible', question.isWhatLinks);
    };

    hideAnswer() {
        $('#answer').hide();
    }

    showAnswer(answer) {
        $('#answer').text(answer).show();
    };

    hideScoreTick() {
        $('#score-tick').hide();
    }

    showScoreTick(score) {
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

        $("#score-tick")
            .removeClass()
            .addClass(scoreClass)
            .show();
    }

    hideScoreShare() {
        $('#score-share').hide();
    }

    showScoreShare() {
        $('#score-share').show();
    }
}
