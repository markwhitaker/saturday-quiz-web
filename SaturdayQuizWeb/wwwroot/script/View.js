class View {
    constructor(presenter) {
        this.presenter = presenter;

        View.#preloadImages(
            "images/score-tick-dark.svg",
            "images/score-tick-light.svg"
        )
    };

    onQuizLoading() {
        $('.page').hide();
    };

    enableNavigation() {
        const presenter = this.presenter;
        $('#nav-left').click(function(e){
            presenter.onPrevious();
            e.preventDefault();
        });
        $('#nav-right').click(function(e){
            presenter.onNext();
            e.preventDefault();
        });
        $(document).keyup(function(e) {
            switch (e.keyCode) {
                case 37: // Left
                    presenter.onPrevious();
                    break;
                case 39: // Right
                    presenter.onNext();
                    break;
                case 32: // Space
                    presenter.toggleScore();
                    break;
                default:
                    return;
            }
            e.preventDefault();
        });

        $('#score-tick').click(function(e){
            presenter.toggleScore();
            e.preventDefault();
        })

        $('#score-share').click(function(e){
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
        $('#score-share').hide();
    };

    showQuestionsTitle(date) {
        const dateString = new Date(date).toLocaleDateString(
            'en-GB',
            {
                day: 'numeric',
                month: 'long',
                year: 'numeric'
            });

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
        $('#score-share').show();
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

    hideScore() {
        $("#score-tick").hide();
    }

    showScore(score) {
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

    static #preloadImages(...images) {
        images.forEach((image) => {
            new Image().src = image;
        })
    }
}
