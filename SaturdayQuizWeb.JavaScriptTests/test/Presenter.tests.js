import { suite, test } from 'mocha';
import assert from 'assert';
import MockNavigatorWrapperBuilder from "./mocks/MockNavigatorWrapperBuilder.js";
import MockQuizRepositoryBuilder from "./mocks/MockQuizRepositoryBuilder.js";
import MockScoreRepositoryBuilder from "./mocks/MockScoreRepositoryBuilder.js";
import MockViewBuilder from "./mocks/MockViewBuilder.js";
import Presenter from "../../SaturdayQuizWeb/wwwroot/script/Presenter.js";
import Question from "../../SaturdayQuizWeb/wwwroot/script/Question.js";
import QuestionScore from "../../SaturdayQuizWeb/wwwroot/script/QuestionScore.js";
import Quiz from "../../SaturdayQuizWeb/wwwroot/script/Quiz.js";

suite('Presenter', () => {
    test('GIVEN quiz repository WHEN view is ready THEN quiz is loaded AND questions title scene is shown', async () => {
        let actualIsQuizStored;
        let actualIsViewNavigationEnabled;
        let actualIsViewQuizLoaded;
        let actualIsViewScoreShareHidden;
        let actualIsViewScoreTickHidden;
        let actualIsViewSkipToAnswersShown;
        let actualIsViewSkipToAnswersToggled;
        let actualIsViewTitlePageShown;
        let actualViewQuestionsTitleDateString;

        const areScoresStored = true;
        const quizDate = new Date('2020-01-02');
        const expectedQuestionsTitleDateString = '2 January 2020';

        const quiz = new Quiz({
            date: quizDate,
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' }),
                new Question({ number: 2, type: 'WHAT_LINKS', question: 'question-2', answer: 'answer-2' })
            ]
        })
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder()
            .getHasScores(() => areScoresStored)
            .initialiseScores(() => actualIsQuizStored = true)
            .build();
        const mockView = new MockViewBuilder()
            .enableNavigation(() => actualIsViewNavigationEnabled = true)
            .hideScoreShare(() => actualIsViewScoreShareHidden = true)
            .hideScoreTick(() => actualIsViewScoreTickHidden = true)
            .onQuizLoaded(() => actualIsViewQuizLoaded = true)
            .setSkipToAnswers(skipToAnswers => actualIsViewSkipToAnswersToggled = skipToAnswers)
            .showQuestionsTitle(dateString => actualViewQuestionsTitleDateString = dateString)
            .showSkipToAnswers(() => actualIsViewSkipToAnswersShown = true)
            .showTitlePage(() => actualIsViewTitlePageShown = true)
            .build();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);

        assert.strictEqual(actualIsQuizStored, true);
        assert.strictEqual(actualIsViewNavigationEnabled, true);
        assert.strictEqual(actualIsViewSkipToAnswersToggled, areScoresStored);
        assert.strictEqual(actualIsViewQuizLoaded, true);
        assert.strictEqual(actualViewQuestionsTitleDateString, expectedQuestionsTitleDateString);
        assert.strictEqual(actualIsViewScoreShareHidden, true);
        assert.strictEqual(actualIsViewScoreTickHidden, true);
        assert.strictEqual(actualIsViewSkipToAnswersShown, true);
        assert.strictEqual(actualIsViewTitlePageShown, true);
    });

    test('GIVEN questions title is shown WHEN on next THEN first question is shown', async () => {
        let actualIsViewAnswerHidden;
        let actualIsViewQuestionPageShown;
        let actualIsViewScoreShareHidden;
        let actualIsViewScoreTickHidden;
        let actualIsViewSkipToAnswersHidden;
        let actualViewQuestion;

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder().build();
        const mockView = new MockViewBuilder().build();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);

        mockView.hideAnswer = () => actualIsViewAnswerHidden = true;
        mockView.hideScoreShare = () => actualIsViewScoreShareHidden = true;
        mockView.hideScoreTick = () => actualIsViewScoreTickHidden = true;
        mockView.hideSkipToAnswers = () => actualIsViewSkipToAnswersHidden = true;
        mockView.showQuestion = question => actualViewQuestion = question;
        mockView.showQuestionPage = () => actualIsViewQuestionPageShown = true;

        presenter.onNext();

        assert.strictEqual(actualIsViewAnswerHidden, true);
        assert.strictEqual(actualIsViewScoreShareHidden, true);
        assert.strictEqual(actualIsViewScoreTickHidden, true);
        assert.strictEqual(actualIsViewSkipToAnswersHidden, true);
        assert.deepStrictEqual(actualViewQuestion, quiz.questions[0]);
        assert.strictEqual(actualIsViewQuestionPageShown, true);
    });

    test('GIVEN questions title is shown WHEN on space THEN skip to answers is toggled', async () => {
        let actualSkipToAnswersValues = [];
        const expectedSkipToAnswersValues = [false, true];

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder()
            .getHasScores(() => false)
            .build();
        const mockView = new MockViewBuilder()
            .setSkipToAnswers(skipToAnswers => actualSkipToAnswersValues.push(skipToAnswers))
            .build();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);

        presenter.onSpace();

        assert.deepStrictEqual(actualSkipToAnswersValues, expectedSkipToAnswersValues);
    });

    test('GIVEN last question is shown WHEN on next THEN answers title is shown', async () => {
        let actualIsViewAnswersTitleShown;
        let actualIsViewScoreShareHidden;
        let actualIsViewScoreTickHidden;
        let actualIsViewSkipToAnswersHidden;
        let actualIsViewTitlePageShown;

        const quiz = new Quiz({
            questions: [
                new Question({number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1'})
            ]
        })
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder().build();
        const mockView = new MockViewBuilder().build();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext();

        mockView.hideScoreShare = () => actualIsViewScoreShareHidden = true;
        mockView.hideScoreTick = () => actualIsViewScoreTickHidden = true;
        mockView.hideSkipToAnswers = () => actualIsViewSkipToAnswersHidden = true;
        mockView.showAnswersTitle = () => actualIsViewAnswersTitleShown = true;
        mockView.showTitlePage = () => actualIsViewTitlePageShown = true;

        presenter.onNext();

        assert.strictEqual(actualIsViewScoreShareHidden, true);
        assert.strictEqual(actualIsViewScoreTickHidden, true);
        assert.strictEqual(actualIsViewSkipToAnswersHidden, true);
        assert.strictEqual(actualIsViewAnswersTitleShown, true);
        assert.strictEqual(actualIsViewTitlePageShown, true);
    });

    test('GIVEN answers title is shown WHEN on next THEN first question is shown', async () => {
        let actualIsViewAnswerHidden;
        let actualIsViewQuestionPageShown;
        let actualIsViewScoreShareHidden;
        let actualIsViewScoreTickHidden;
        let actualIsViewSkipToAnswersHidden;
        let actualViewQuestion;

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder().build();
        const mockView = new MockViewBuilder().build();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext(); // show first question
        presenter.onNext(); // show answers title

        mockView.hideAnswer = () => actualIsViewAnswerHidden = true;
        mockView.hideScoreShare = () => actualIsViewScoreShareHidden = true;
        mockView.hideScoreTick = () => actualIsViewScoreTickHidden = true;
        mockView.hideSkipToAnswers = () => actualIsViewSkipToAnswersHidden = true;
        mockView.showQuestion = question => actualViewQuestion = question;
        mockView.showQuestionPage = () => actualIsViewQuestionPageShown = true;

        presenter.onNext(); // show first question again

        assert.strictEqual(actualIsViewAnswerHidden, true);
        assert.strictEqual(actualIsViewScoreShareHidden, true);
        assert.strictEqual(actualIsViewScoreTickHidden, true);
        assert.strictEqual(actualIsViewSkipToAnswersHidden, true);
        assert.deepStrictEqual(actualViewQuestion, quiz.questions[0]);
        assert.strictEqual(actualIsViewQuestionPageShown, true);
    });

    test('GIVEN question is shown for a second time WHEN on next THEN first answer is shown', async () => {
        let actualIsViewAnswerShown;
        let actualIsViewQuestionPageShown;
        let actualIsViewScoreShareHidden;
        let actualIsViewScoreTickShown;
        let actualIsViewSkipToAnswersHidden;
        let actualViewQuestion;

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder().build();
        const mockView = new MockViewBuilder().build();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext(); // show first question
        presenter.onNext(); // show answers title
        presenter.onNext(); // show first question again

        mockView.hideScoreShare = () => actualIsViewScoreShareHidden = true;
        mockView.hideSkipToAnswers = () => actualIsViewSkipToAnswersHidden = true;
        mockView.showAnswer = () => actualIsViewAnswerShown = true;
        mockView.showQuestion = question => actualViewQuestion = question;
        mockView.showQuestionPage = () => actualIsViewQuestionPageShown = true;
        mockView.showScoreTick = () => actualIsViewScoreTickShown = true;

        presenter.onNext(); // show first answer

        assert.strictEqual(actualIsViewAnswerShown, true);
        assert.strictEqual(actualIsViewScoreShareHidden, true);
        assert.strictEqual(actualIsViewScoreTickShown, true);
        assert.strictEqual(actualIsViewSkipToAnswersHidden, true);
        assert.deepStrictEqual(actualViewQuestion, quiz.questions[0]);
        assert.strictEqual(actualIsViewQuestionPageShown, true);
    });

    test('GIVEN answer is shown WHEN on space THEN score is toggled', async () => {
        let storedQuestionScore = QuestionScore.NONE;
        let actualQuestionScoreSequence = [];
        let expectedQuestionScoreSequence = [
            QuestionScore.NONE,
            QuestionScore.FULL,
            QuestionScore.HALF,
            QuestionScore.NONE
        ];

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockNavigatorWrapper = new MockNavigatorWrapperBuilder()
            .isShareSupported(() => true)
            .build();
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder()
            .setScore((questionNumber, score) => storedQuestionScore = score)
            .getScore(() => storedQuestionScore)
            .build();
        const mockView = new MockViewBuilder()
            .showScoreTick(score => actualQuestionScoreSequence.push(score))
            .build();

        const presenter = new Presenter({
            navigatorWrapper: mockNavigatorWrapper,
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext(); // show first question
        presenter.onNext(); // show answers title
        presenter.onNext(); // show first question again
        presenter.onNext(); // show first answer

        presenter.onSpace();
        presenter.onSpace();
        presenter.onSpace();

        assert.deepStrictEqual(actualQuestionScoreSequence, expectedQuestionScoreSequence);
    });

    test('GIVEN last answer is shown WHEN on next THEN end title is shown', async () => {
        let actualIsViewEndTitleShown;
        let actualIsViewTitlePageShown;
        let actualIsViewScoreShareShown;
        let actualIsViewScoreTickHidden;
        let actualIsViewSkipToAnswersHidden;

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockNavigatorWrapper = new MockNavigatorWrapperBuilder()
            .isShareSupported(() => true)
            .build();
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder().build();
        const mockView = new MockViewBuilder().build();

        const presenter = new Presenter({
            navigatorWrapper: mockNavigatorWrapper,
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext(); // show first question
        presenter.onNext(); // show answers title
        presenter.onNext(); // show first question again
        presenter.onNext(); // show first answer

        mockView.hideScoreTick = () => actualIsViewScoreTickHidden = true;
        mockView.hideSkipToAnswers = () => actualIsViewSkipToAnswersHidden = true;
        mockView.showEndTitle = () => actualIsViewEndTitleShown = true;
        mockView.showScoreShare = () => actualIsViewScoreShareShown = true;
        mockView.showTitlePage = question => actualIsViewTitlePageShown = true;

        presenter.onNext();

        assert.strictEqual(actualIsViewScoreTickHidden, true);
        assert.strictEqual(actualIsViewSkipToAnswersHidden, true);
        assert.strictEqual(actualIsViewEndTitleShown, true);
        assert.strictEqual(actualIsViewScoreShareShown, true);
        assert.deepStrictEqual(actualIsViewTitlePageShown, true);
    });

    test('GIVEN end title is shown WHEN on previous THEN last answer is shown', async () => {
        let actualIsViewAnswerShown;
        let actualIsViewQuestionPageShown;
        let actualIsViewScoreShareHidden;
        let actualIsViewScoreTickShown;
        let actualIsViewSkipToAnswersHidden;
        let actualViewQuestion;

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockNavigatorWrapper = new MockNavigatorWrapperBuilder()
            .isShareSupported(() => true)
            .build();
        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => (quiz))
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder().build();
        const mockView = new MockViewBuilder().build();

        const presenter = new Presenter({
            navigatorWrapper: mockNavigatorWrapper,
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext(); // show first question
        presenter.onNext(); // show answers title
        presenter.onNext(); // show first question again
        presenter.onNext(); // show first answer
        presenter.onNext(); // show end title

        mockView.hideScoreShare = () => actualIsViewScoreShareHidden = true;
        mockView.hideSkipToAnswers = () => actualIsViewSkipToAnswersHidden = true;
        mockView.showAnswer = () => actualIsViewAnswerShown = true;
        mockView.showQuestion = question => actualViewQuestion = question;
        mockView.showQuestionPage = () => actualIsViewQuestionPageShown = true;
        mockView.showScoreTick = () => actualIsViewScoreTickShown = true;

        presenter.onPrevious();

        assert.strictEqual(actualIsViewAnswerShown, true);
        assert.strictEqual(actualIsViewScoreShareHidden, true);
        assert.strictEqual(actualIsViewScoreTickShown, true);
        assert.strictEqual(actualIsViewSkipToAnswersHidden, true);
        assert.deepStrictEqual(actualViewQuestion, quiz.questions[0]);
        assert.strictEqual(actualIsViewQuestionPageShown, true);
    });

    test('GIVEN score repository has scores WHEN share score THEN expected data is shared', async () => {
        let actualSharedData;

        const allScores = [QuestionScore.NONE, QuestionScore.HALF, QuestionScore.FULL];
        const totalScore = 1.5;

        const expectedSharedData = {
            title: 'QUIZ RESULTS',
            text: 'We have quizzed! Our total score this week is 1½...\n\n' +
                  '2 (half), 3'
        };

        const mockScoreRepository = new MockScoreRepositoryBuilder()
            .getAllScores(() => allScores)
            .getTotalScore(() => totalScore)
            .build();

        const mockNavigatorWrapper = new MockNavigatorWrapperBuilder()
            .share(async data => actualSharedData = data)
            .build();

        const presenter = new Presenter({
            scoreRepository: mockScoreRepository,
            navigatorWrapper: mockNavigatorWrapper
        });

        await presenter.shareScore();

        assert.deepStrictEqual(actualSharedData, expectedSharedData);
    });
});
