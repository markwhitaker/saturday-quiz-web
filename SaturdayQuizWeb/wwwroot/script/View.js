class View {
    setController(controller) {
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
    };

    showAnswersTitle() {
        $('#title').text('Answers');
        $('#quiz-date').text('');
    };

    showEndTitle() {
        $('#title').text('End');
        $('#quiz-date').text('');
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
}
