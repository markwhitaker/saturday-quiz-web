import { suite, test } from 'mocha';
import assert from 'assert';
import MockQuizRepositoryBuilder from "./mocks/MockQuizRepositoryBuilder.js";
import MockScoreRepositoryBuilder from "./mocks/MockScoreRepositoryBuilder.js";
import MockViewBuilder from "./mocks/MockViewBuilder.js";
import Presenter from "../../SaturdayQuizWeb/wwwroot/script/Presenter.js";
import Question from "../../SaturdayQuizWeb/wwwroot/script/Question.js";
import Quiz from "../../SaturdayQuizWeb/wwwroot/script/Quiz.js";

suite('Presenter', () => {
    test('GIVEN quiz repository WHEN view is ready THEN quiz is loaded', async () => {
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
});
