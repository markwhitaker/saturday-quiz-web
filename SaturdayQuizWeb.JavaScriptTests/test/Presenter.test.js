import { describe, test, expect, mock } from 'bun:test';
import Presenter from "../../SaturdayQuizWeb/wwwroot/script/Presenter.js";
import Question from "../../SaturdayQuizWeb/wwwroot/script/Question.js";
import QuestionScore from "../../SaturdayQuizWeb/wwwroot/script/QuestionScore.js";
import Quiz from "../../SaturdayQuizWeb/wwwroot/script/Quiz.js";

function createMockView(overrides = {}) {
    return {
        enableNavigation: mock(),
        hideAnswer: mock(),
        hideScoreShare: mock(),
        hideScoreTick: mock(),
        hideSkipToAnswers: mock(),
        onNext: mock(),
        onPrevious: mock(),
        onQuizLoaded: mock(),
        onSpace: mock(),
        setSkipToAnswers: mock(),
        showAnswer: mock(),
        showAnswersTitle: mock(),
        showEndTitle: mock(),
        showQuestion: mock(),
        showQuestionPage: mock(),
        showQuestionsTitle: mock(),
        showScoreShare: mock(),
        showScoreTick: mock(),
        showSkipToAnswers: mock(),
        showTitlePage: mock(),
        toggleScore: mock(),
        ...overrides
    };
}

describe('Presenter', () => {
    test('GIVEN quiz repository WHEN view is ready THEN quiz is loaded AND questions title scene is shown', async () => {
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
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(() => areScoresStored),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);

        expect(mockScoreRepository.initialiseScores).toHaveBeenCalled();
        expect(mockView.enableNavigation).toHaveBeenCalled();
        expect(mockView.setSkipToAnswers).toHaveBeenCalledWith(areScoresStored);
        expect(mockView.onQuizLoaded).toHaveBeenCalled();
        expect(mockView.showQuestionsTitle).toHaveBeenCalledWith(expectedQuestionsTitleDateString);
        expect(mockView.hideScoreShare).toHaveBeenCalled();
        expect(mockView.hideScoreTick).toHaveBeenCalled();
        expect(mockView.showSkipToAnswers).toHaveBeenCalled();
        expect(mockView.showTitlePage).toHaveBeenCalled();
    });

    test('GIVEN questions title is shown WHEN on next THEN first question is shown', async () => {
        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);

        presenter.onNext();

        expect(mockView.hideAnswer).toHaveBeenCalled();
        expect(mockView.hideScoreShare).toHaveBeenCalled();
        expect(mockView.hideScoreTick).toHaveBeenCalled();
        expect(mockView.hideSkipToAnswers).toHaveBeenCalled();
        expect(mockView.showQuestion).toHaveBeenCalledWith(quiz.getQuestions()[0]);
        expect(mockView.showQuestionPage).toHaveBeenCalled();
    });

    test('GIVEN questions title is shown WHEN on space THEN skip to answers is toggled', async () => {
        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(() => false),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);

        presenter.onSpace();

        expect(mockView.setSkipToAnswers).toHaveBeenCalledTimes(2);
        expect(mockView.setSkipToAnswers.mock.calls[0][0]).toBe(false);
        expect(mockView.setSkipToAnswers.mock.calls[1][0]).toBe(true);
    });

    test('GIVEN last question is shown WHEN on next THEN answers title is shown', async () => {
        const quiz = new Quiz({
            questions: [
                new Question({number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1'})
            ]
        })
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext();

        presenter.onNext();

        expect(mockView.hideScoreShare).toHaveBeenCalled();
        expect(mockView.hideScoreTick).toHaveBeenCalled();
        expect(mockView.hideSkipToAnswers).toHaveBeenCalled();
        expect(mockView.showAnswersTitle).toHaveBeenCalled();
        expect(mockView.showTitlePage).toHaveBeenCalled();
    });

    test('GIVEN answers title is shown WHEN on next THEN first question is shown', async () => {
        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext(); // show first question
        presenter.onNext(); // show answers title

        presenter.onNext(); // show first question again

        expect(mockView.hideAnswer).toHaveBeenCalled();
        expect(mockView.hideScoreShare).toHaveBeenCalled();
        expect(mockView.hideScoreTick).toHaveBeenCalled();
        expect(mockView.hideSkipToAnswers).toHaveBeenCalled();
        expect(mockView.showQuestion).toHaveBeenCalledWith(quiz.getQuestions()[0]);
        expect(mockView.showQuestionPage).toHaveBeenCalled();
    });

    test('GIVEN question is shown for a second time WHEN on next THEN first answer is shown', async () => {
        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        await presenter.onViewReady(mockView);
        presenter.onNext(); // show first question
        presenter.onNext(); // show answers title
        presenter.onNext(); // show first question again

        presenter.onNext(); // show first answer

        expect(mockView.showAnswer).toHaveBeenCalled();
        expect(mockView.hideScoreShare).toHaveBeenCalled();
        expect(mockView.showScoreTick).toHaveBeenCalled();
        expect(mockView.hideSkipToAnswers).toHaveBeenCalled();
        expect(mockView.showQuestion).toHaveBeenCalledWith(quiz.getQuestions()[0]);
        expect(mockView.showQuestionPage).toHaveBeenCalled();
    });

    test('GIVEN answer is shown WHEN on space THEN score is toggled', async () => {
        let storedQuestionScore = QuestionScore.NONE;

        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockNavigatorWrapper = {
            isShareSupported: mock(() => true),
            share: mock()
        };
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(),
            getScore: mock(() => storedQuestionScore),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock((questionNumber, score) => storedQuestionScore = score)
        };
        const mockView = createMockView();

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

        expect(mockView.showScoreTick).toHaveBeenCalledTimes(4);
        expect(mockView.showScoreTick.mock.calls[0][0]).toBe(QuestionScore.NONE);
        expect(mockView.showScoreTick.mock.calls[1][0]).toBe(QuestionScore.FULL);
        expect(mockView.showScoreTick.mock.calls[2][0]).toBe(QuestionScore.HALF);
        expect(mockView.showScoreTick.mock.calls[3][0]).toBe(QuestionScore.NONE);
    });

    test('GIVEN last answer is shown WHEN on next THEN end title is shown', async () => {
        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockNavigatorWrapper = {
            isShareSupported: mock(() => true),
            share: mock()
        };
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

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

        presenter.onNext();

        expect(mockView.hideScoreTick).toHaveBeenCalled();
        expect(mockView.hideSkipToAnswers).toHaveBeenCalled();
        expect(mockView.showEndTitle).toHaveBeenCalled();
        expect(mockView.showScoreShare).toHaveBeenCalled();
        expect(mockView.showTitlePage).toHaveBeenCalled();
    });

    test('GIVEN end title is shown WHEN on previous THEN last answer is shown', async () => {
        const quiz = new Quiz({
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' })
            ]
        })
        const mockNavigatorWrapper = {
            isShareSupported: mock(() => true),
            share: mock()
        };
        const mockQuizRepository = {
            loadLatestQuiz: mock(async () => quiz)
        };
        const mockScoreRepository = {
            getAllScores: mock(),
            hasScores: mock(),
            getScore: mock(),
            getTotalScore: mock(),
            initialiseScores: mock(),
            setScore: mock()
        };
        const mockView = createMockView();

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

        presenter.onPrevious();

        expect(mockView.showAnswer).toHaveBeenCalled();
        expect(mockView.hideScoreShare).toHaveBeenCalled();
        expect(mockView.showScoreTick).toHaveBeenCalled();
        expect(mockView.hideSkipToAnswers).toHaveBeenCalled();
        expect(mockView.showQuestion).toHaveBeenCalledWith(quiz.getQuestions()[0]);
        expect(mockView.showQuestionPage).toHaveBeenCalled();
    });

    test('GIVEN score repository has scores WHEN share score THEN expected data is shared', async () => {
        const allScores = [QuestionScore.NONE, QuestionScore.HALF, QuestionScore.FULL];
        const totalScore = 1.5;

        const expectedSharedData = {
            text: '1½...\n\n' +
                  '2 (half), 3'
        };

        const mockScoreRepository = {
            getAllScores: mock(() => allScores),
            hasScores: mock(),
            getScore: mock(),
            getTotalScore: mock(() => totalScore),
            initialiseScores: mock(),
            setScore: mock()
        };

        const mockNavigatorWrapper = {
            isShareSupported: mock(),
            share: mock()
        };

        const presenter = new Presenter({
            scoreRepository: mockScoreRepository,
            navigatorWrapper: mockNavigatorWrapper
        });

        await presenter.shareScore();

        expect(mockNavigatorWrapper.share).toHaveBeenCalledWith(expectedSharedData);
    });
});
