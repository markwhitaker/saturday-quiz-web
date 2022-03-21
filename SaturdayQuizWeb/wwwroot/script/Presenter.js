const QuestionType = Object.freeze({
    NORMAL: 'NORMAL',
    WHAT_LINKS: 'WHAT_LINKS'
});

class Presenter {
    constructor(scoreRepository) {
        this.sceneIndex = 0;
        this.scoreRepository = scoreRepository;
    }

    onViewReady(view) {
        this.view = view;
        this.view.onQuizLoading();
        this.loadQuiz();
    };

    onNext() {
        if (this.sceneIndex < this.scenes.length - 1) {
            this.sceneIndex++;
            this.showScene();
        }
    };

    onPrevious() {
        if (this.sceneIndex > 0) {
            this.sceneIndex--;
            this.showScene();
        }
    };

    loadQuiz() {
        const _this = this;
        $.get("api/quiz", function (quizObject) {
            _this.onQuizLoaded(new Quiz(quizObject));
        });
    };

    onQuizLoaded(quiz) {
        this.scoreRepository.initialiseScores(quiz);
        this.scenes = Presenter.#buildScenes(quiz, this.scoreRepository.hasScores);
        this.showScene();
        this.view.enableNavigation();
    };

    showScene() {
        const scene = this.scenes[this.sceneIndex];
        const question = scene.question;
        const view = this.view;

        switch(scene.type) {
            case SceneType.QUESTIONS_TITLE:
                view.hideScore();
                view.showQuestionsTitle(scene.date);
                view.showTitlePage();
                break;
            case SceneType.QUESTION:
                view.hideScore();
                view.showQuestionNumber(question.number);
                view.showQuestion(
                    question.question,
                    question.type === QuestionType.WHAT_LINKS
                );
                view.showAnswer('');
                view.showQuestionPage();
                break;
            case SceneType.ANSWERS_TITLE:
                view.hideScore();
                view.showTitlePage();
                view.showAnswersTitle();
                break;
            case SceneType.QUESTION_ANSWER:
                view.showQuestionNumber(question.number);
                view.showQuestion(
                    question.question,
                    question.type === QuestionType.WHAT_LINKS
                );
                view.showAnswer(question.answer);
                view.showQuestionPage();
                view.showScore(this.scoreRepository.getScore(question.number));
                break;
            case SceneType.END_TITLE:
                view.hideScore();
                view.showEndTitle(this.scoreRepository.totalScore);
                view.showTitlePage();
                break;
        }
    };

    toggleScore() {
        const scene = this.scenes[this.sceneIndex];
        if (scene.type !== SceneType.QUESTION_ANSWER) {
            return;
        }

        const questionNumber = scene.question.number;
        let score = this.scoreRepository.getScore(questionNumber);
        switch (score)
        {
            case QuestionScore.NONE:
                score = QuestionScore.FULL;
                break;
            case QuestionScore.FULL:
                score = QuestionScore.HALF;
                break;
            case QuestionScore.HALF:
                score = QuestionScore.NONE;
                break;
        }
        this.scoreRepository.setScore(questionNumber, score);
        this.view.showScore(score);
    }

    static #buildScenes(quiz, jumpToAnswers) {
        let i;
        const scenes = [];

        scenes.push(new Scene(SceneType.QUESTIONS_TITLE, quiz.date));

        if (!jumpToAnswers) {
            // First just show the questions
            for (i = 0; i < quiz.questions.length; i++) {
                scenes.push(new Scene(SceneType.QUESTION, null, quiz.questions[i]));
            }
        }

        scenes.push(new Scene(SceneType.ANSWERS_TITLE));

        // Now recap the questions, showing the answer after each one
        for (i = 0; i < quiz.questions.length; i++) {
            scenes.push(new Scene(SceneType.QUESTION, null, quiz.questions[i]));
            scenes.push(new Scene(SceneType.QUESTION_ANSWER, null, quiz.questions[i]));
        }

        scenes.push(new Scene(SceneType.END_TITLE));

        return scenes;
    }
}
