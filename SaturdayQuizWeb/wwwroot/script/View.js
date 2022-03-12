class View {
    constructor(controller) {
        this.controller = controller;
    };

    onQuizLoading() {
        $('.page').hide();
    };

    enableNavigation() {
        const controller = this.controller;
        $('#nav-left').click(function(){
            controller.onPrevious();
        });
        $('#nav-right').click(function(){
            controller.onNext();
        });
        $(document).keyup(function(e) {
            switch (e.keyCode) {
                case 37: // Left
                    controller.onPrevious();
                    break;
                case 39: // Right
                    controller.onNext();
                    break;
                default:
                    return;
            }
            e.preventDefault();
        });

        $("#score").click(function(){
            controller.toggleScore();
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

        let totalScoreString = Math.floor(totalScore);
        if (totalScore % 1 === 0.5) {
            totalScoreString += "½"
        }
        $('#total-score').text("Total score: " + totalScoreString);
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
        $("#score").hide();
    }

    showScore(score) {
        $("#score").text(score).show();
    }
}
