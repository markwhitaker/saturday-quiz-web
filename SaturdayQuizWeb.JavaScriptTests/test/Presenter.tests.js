import { suite, test } from 'mocha';
import assert from 'assert';
import MockQuizRepositoryBuilder from "./mocks/MockQuizRepositoryBuilder.js";
import MockScoreRepositoryBuilder from "./mocks/MockScoreRepositoryBuilder.js";
import MockViewBuilder from "./mocks/MockViewBuilder.js";
import Presenter from "../../SaturdayQuizWeb/wwwroot/script/Presenter.js";
import Question from "../../SaturdayQuizWeb/wwwroot/script/Question.js";
import Quiz from "../../SaturdayQuizWeb/wwwroot/script/Quiz.js";

suite('Presenter', () => {
    test('GIVEN quiz repository WHEN view is ready THEN quiz is loaded', () => {
        let isQuizStored;
        let isViewNavigationEnabled;
        let isViewSkipToAnswersToggled;
        let isViewQuizLoaded;

        const quiz = new Quiz({
            date: '2020-01-02',
            questions: [
                new Question({ number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' }),
                new Question({ number: 2, type: 'WHAT_LINKS', question: 'question-2', answer: 'answer-2' })
            ]
        })

        const mockQuizRepository = new MockQuizRepositoryBuilder()
            .loadLatestQuiz(async () => quiz)
            .build();
        const mockScoreRepository = new MockScoreRepositoryBuilder()
            .initialiseScores(() => isQuizStored = true)
            .build();
        const mockView = new MockViewBuilder()
            .enableNavigation(() => isViewNavigationEnabled = true)
            .setSkipToAnswers(skipToAnswers => isViewSkipToAnswersToggled = skipToAnswers)
            .onQuizLoaded(() => isViewQuizLoaded = true)
            .build();

        const presenter = new Presenter({
            quizRepository: mockQuizRepository,
            scoreRepository: mockScoreRepository
        });

        presenter.onViewReady(mockView);

        assert.strictEqual(isQuizStored, true);
        assert.strictEqual(isViewNavigationEnabled, true);
        assert.strictEqual(isViewSkipToAnswersToggled, false);
        assert.strictEqual(isViewQuizLoaded, true);
    });
});
