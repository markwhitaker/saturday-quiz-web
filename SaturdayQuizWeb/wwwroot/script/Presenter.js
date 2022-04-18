class Presenter {
    constructor(scoreRepository) {
        this.scoreRepository = scoreRepository;
        this.sceneIndex = 0;
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
        this.view.reveal();
    };

    showScene() {
        const scene = this.scenes[this.sceneIndex];
        const question = scene.question;
        const view = this.view;

        switch(scene.type) {
            case SceneType.QUESTIONS_TITLE:
                let dateString = scene.date.toLocaleDateString("en-GB", {
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                });
                view.hideScoreTick();
                view.hideScoreShare();
                view.showQuestionsTitle(dateString);
                view.showTitlePage();
                break;
            case SceneType.QUESTION:
                view.hideScoreTick();
                view.hideScoreShare();
                view.showQuestionNumber(question.number);
                view.showQuestion(
                    question.question,
                    question.type === QuestionType.WHAT_LINKS
                );
                view.showAnswer('');
                view.showQuestionPage();
                break;
            case SceneType.ANSWERS_TITLE:
                view.hideScoreTick();
                view.hideScoreShare();
                view.showTitlePage();
                view.showAnswersTitle();
                break;
            case SceneType.QUESTION_ANSWER:
                view.hideScoreShare();
                view.showQuestionNumber(question.number);
                view.showQuestion(
                    question.question,
                    question.type === QuestionType.WHAT_LINKS
                );
                view.showAnswer(question.answer);
                view.showQuestionPage();
                view.showScoreTick(this.scoreRepository.getScore(question.number));
                break;
            case SceneType.END_TITLE:
                view.hideScoreTick();
                view.showEndTitle(Presenter.#formatTotalScore(this.scoreRepository.totalScore));
                view.showTitlePage();
                view.showScoreShare();
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
        this.view.showScoreTick(score);
    }

    shareScore() {
        let totalScore = Presenter.#formatTotalScore(this.scoreRepository.totalScore);
        let scoreBreakdown = this.scoreRepository.allScores
            .map((score, index) =>
                score === QuestionScore.NONE ? null :
                score === QuestionScore.HALF ? (index + 1) + ' (half)' :
                '' + (index + 1)
            )
            .filter(scoreText => scoreText !== null)
            .join(', ');

        let shareObject = {
            title: 'QUIZ RESULTS',
            text: 'We have quizzed! Total score this week is ' + totalScore + '...\n\n' + scoreBreakdown
        };
        if (navigator.canShare && navigator.canShare(shareObject)) {
            navigator
                .share(shareObject)
                .then(() => console.log('Shared score'))
                .catch((error) => console.log('Sharing score failed', error));
        }
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

    static #formatTotalScore(totalScore) {
        let totalScoreString = Math.floor(totalScore);
        if (totalScore % 1 === QuestionScore.HALF) {
            totalScoreString += "½"
        }
        return totalScoreString;
    }
}
