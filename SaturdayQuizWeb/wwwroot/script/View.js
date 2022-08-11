class View {
    constructor(presenter) {
        this.presenter = presenter;
    };

    onQuizLoading() {
        $('.page').hide();
    };

    reveal() {
        $('#box').removeClass('hidden');
    }

    enableNavigation() {
        const presenter = this.presenter;
        $('#nav-left').click(e => {
            presenter.onPrevious();
            e.preventDefault();
        });
        $('#nav-right').click(e => {
            presenter.onNext();
            e.preventDefault();
        });
        $(document).keyup(e => {
            switch (e.code) {
                case 'ArrowLeft':
                    presenter.onPrevious();
                    break;
                case 'ArrowRight':
                    presenter.onNext();
                    break;
                case 'Space':
                    presenter.toggleScore();
                    break;
                default:
                    return;
            }
            e.preventDefault();
        });

        $('#score-tick').click(e => {
            presenter.toggleScore();
            e.preventDefault();
        })

        $('#score-share').click(e => {
            presenter.shareScore();
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
