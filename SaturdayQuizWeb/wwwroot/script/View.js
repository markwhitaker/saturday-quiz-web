﻿import QuestionScore from "./QuestionScore.js";

export default class View {
    constructor(presenter) {
        this.presenter = presenter;
    };

    onQuizLoaded() {
        $('#loader-box').fadeOut(function(){
            $('#box').removeClass('hidden');
        });
    }

    enableNavigation() {
        $('#nav-left').click(e => {
            this.presenter.onPrevious();
            e.preventDefault();
        });
        $('#nav-right').click(e => {
            this.presenter.onNext();
            e.preventDefault();
        });
        $(document).keyup(e => {
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

        $('#score-tick').click(e => {
            this.presenter.toggleScore();
            e.preventDefault();
        })

        $('#score-share').click(async e => {
            await this.presenter.shareScore();
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

    showQuestionNumber(number) {
        $('#question-number').text(number + '.');
    };

    showQuestion(question, isWhatLinks) {
        $('#question').text(question);
        $('#question-what-links').toggleClass('visible', isWhatLinks);
    };

    showAnswer(answer) {
        $('#answer').text(answer);
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
