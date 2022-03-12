const SceneType = Object.freeze({
    QUESTIONS_TITLE: 0,
    QUESTION: 1,
    ANSWERS_TITLE: 2,
    QUESTION_ANSWER: 3,
    END_TITLE: 4
});

const QuestionType = Object.freeze({
    NORMAL: 'NORMAL',
    WHAT_LINKS: 'WHAT_LINKS'
});

class Controller {
    constructor(scoreRepository) {
        this.sceneIndex = 0;
        this.scoreRepository = scoreRepository;
    }

    onViewReady(view) {
        this.view = view;
        this.view.setController(this);
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
        $.get("api/quiz", function (quiz) {
            _this.onQuizLoaded(quiz);
        });
    };

    onQuizLoaded(quiz) {
        this.scoreRepository.initialiseScores(quiz);
        this.scenes = Controller.#buildScenes(quiz);
        this.showScene();
        this.view.enableNavigation();
    };

    showScene() {
        const scene = this.scenes[this.sceneIndex];
        const question = scene.question;
        const view = this.view;

        switch(scene.type) {
            case SceneType.QUESTIONS_TITLE:
                view.showQuestionsTitle(scene.date);
                view.showTitlePage();
                break;
            case SceneType.QUESTION:
                view.showQuestionNumber(question.number);
                view.showQuestion(
                    question.question,
                    question.type === QuestionType.WHAT_LINKS
                );
                view.showAnswer('');
                view.showQuestionPage();
                break;
            case SceneType.ANSWERS_TITLE:
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
                break;
            case SceneType.END_TITLE:
                view.showEndTitle();
                view.showTitlePage();
                break;
        }
    };

    static #buildScenes(quiz) {
        let i;
        const scenes = [];
        // First just show the questions
        scenes.push({
            type: SceneType.QUESTIONS_TITLE,
            date: quiz.date
        });
        for (i = 0; i < quiz.questions.length; i++) {
            scenes.push({
                type: SceneType.QUESTION,
                question: quiz.questions[i]
            });
        }
        scenes.push({
            type: SceneType.ANSWERS_TITLE
        });
        // Now recap the questions, showing the answer after each one
        for (i = 0; i < quiz.questions.length; i++) {
            scenes.push({
                type: SceneType.QUESTION,
                question: quiz.questions[i]
            });
            scenes.push({
                type: SceneType.QUESTION_ANSWER,
                question: quiz.questions[i]
            });
        }
        scenes.push({
            type: SceneType.END_TITLE
        });

        return scenes;
    }
}
