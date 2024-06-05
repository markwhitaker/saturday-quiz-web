export default class ViewMockBuilder {
    #mock = {
        enableNavigation: () => {},
        hideAnswer: () => {},
        hideScoreShare: () => {},
        hideScoreTick: () => {},
        hideSkipToAnswers: () => {},
        onNext: () => {},
        onPrevious: () => {},
        onQuizLoaded: () => {},
        onSpace: () => {},
        setSkipToAnswers: () => {},
        showAnswer: () => {},
        showAnswersTitle: () => {},
        showEndTitle: () => {},
        showQuestion: () => {},
        showQuestionPage: () => {},
        showQuestionsTitle: () => {},
        showScoreTick: () => {},
        showSkipToAnswers: () => {},
        showTitlePage: () => {},
        toggleScore: () => {}
    }

    enableNavigation(fn) {
        this.#mock.enableNavigation = fn;
        return this;
    }

    hideAnswer(fn) {
        this.#mock.hideAnswer = fn;
        return this;
    }

    hideScoreShare(fn) {
        this.#mock.hideScoreShare = fn;
        return this;
    }

    hideScoreTick(fn) {
        this.#mock.hideScoreTick = fn;
        return this;
    }

    hideSkipToAnswers(fn) {
        this.#mock.hideSkipToAnswers = fn;
        return this;
    }

    onNext(fn) {
        this.#mock.onNext = fn;
        return this;
    }

    onPrevious(fn) {
        this.#mock.onPrevious = fn;
        return this;
    }

    onQuizLoaded(fn) {
        this.#mock.onQuizLoaded = fn;
        return this;
    }

    onSpace(fn) {
        this.#mock.onSpace = fn;
        return this;
    }

    setSkipToAnswers(fn) {
        this.#mock.setSkipToAnswers = fn;
        return this;
    }

    showAnswer(fn) {
        this.#mock.showAnswer = fn;
        return this;
    }

    showAnswersTitle(fn) {
        this.#mock.showAnswersTitle = fn;
        return this;
    }

    showEndTitle(fn) {
        this.#mock.showEndTitle = fn;
        return this;
    }

    showQuestion(fn) {
        this.#mock.showQuestion = fn;
        return this;
    }

    showQuestionPage(fn) {
        this.#mock.showQuestionPage = fn;
        return this;
    }

    showQuestionsTitle(fn) {
        this.#mock.showQuestionsTitle = fn;
        return this;
    }

    showScoreTick(fn) {
        this.#mock.showScoreTick = fn;
        return this;
    }

    showSkipToAnswers(fn) {
        this.#mock.showSkipToAnswers = fn;
        return this;
    }

    showTitlePage(fn) {
        this.#mock.showTitlePage = fn;
        return this;
    }

    toggleScore(fn) {
        this.#mock.toggleScore = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
