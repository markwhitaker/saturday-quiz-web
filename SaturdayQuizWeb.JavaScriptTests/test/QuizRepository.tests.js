import { suite, test } from 'mocha';
import assert from 'assert';
import CalendarDate from "../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js";
import LocalStoreMockBuilder from "./mocks/LocalStorageMockBuilder.js";
import QuizRepository from "../../SaturdayQuizWeb/wwwroot/script/QuizRepository.js";
import Quiz from "../../SaturdayQuizWeb/wwwroot/script/Quiz.js";
import FetchWrapperMockBuilder from "./mocks/FetchWrapperMockBuilder.js";

suite('QuizRepository', function() {
    test('GIVEN quiz is cached and less than 7 days old WHEN load latest quiz THEN cached quiz is returned', function() {
        const cachedQuizDate = new CalendarDate(new Date().getDate() - 6);
        const cachedRawQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };
        const expectedQuiz = new Quiz(cachedRawQuiz);
        const mockLocalStore = new LocalStoreMockBuilder()
            .getQuizDate(() => cachedQuizDate)
            .getQuiz(() => cachedRawQuiz)
            .build();
        const quizRepository = new QuizRepository({
            localStore: mockLocalStore
        });

        quizRepository.loadLatestQuiz().then(actualQuiz =>
            assert.deepStrictEqual(actualQuiz, expectedQuiz)
        );
    });

    test('GIVEN quiz is cached and more than 7 days old WHEN load latest quiz THEN cached quiz is cleared and new quiz fetched', async () => {
        const cachedQuizDate = new CalendarDate(new Date().getDate() - 8);
        const cachedRawQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'cached-question', 'answer': 'cached-answer' }]
        };
        const fetchedRawQuiz = {
            'date': '2021-02-03',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'fetched-question', 'answer': 'fetched-answer' }]
        };

        const expectedStoredQuizDate = new CalendarDate(new Date(2021, 1, 3));
        const expectedQuiz = new Quiz(fetchedRawQuiz);

        let actualIsQuizCleared = undefined;
        let actualStoredQuiz = undefined;
        let actualStoredQuizDate = undefined;

        const mockLocalStore = new LocalStoreMockBuilder()
            .getQuizDate(() => cachedQuizDate)
            .getQuiz(() => cachedRawQuiz)
            .clearQuiz(() => actualIsQuizCleared = true)
            .setQuiz(quiz => actualStoredQuiz = quiz)
            .setQuizDate(quizDate => actualStoredQuizDate = quizDate)
            .build();
        const mockFetchWrapper = new FetchWrapperMockBuilder()
            .fetch(async () => ({
                ok: true,
                json: async() => fetchedRawQuiz
            }))
            .build();
        const quizRepository = new QuizRepository({
            fetchWrapper: mockFetchWrapper,
            localStore: mockLocalStore
        });

        const actualQuiz = await quizRepository.loadLatestQuiz();

        assert.deepStrictEqual(actualQuiz, expectedQuiz);
        assert.deepStrictEqual(actualStoredQuiz, fetchedRawQuiz);
        assert.deepStrictEqual(actualStoredQuizDate, expectedStoredQuizDate);
        assert.strictEqual(actualIsQuizCleared, true);
    });

    test('GIVEN quiz is not cached WHEN load latest quiz THEN cached quiz is cleared and new quiz fetched', async () => {
        const fetchedRawQuiz = {
            'date': '2021-02-03',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'fetched-question', 'answer': 'fetched-answer' }]
        };

        const expectedStoredQuizDate = new CalendarDate(new Date(2021, 1, 3));
        const expectedQuiz = new Quiz(fetchedRawQuiz);

        let actualIsQuizCleared = undefined;
        let actualStoredQuiz = undefined;
        let actualStoredQuizDate = undefined;

        const mockLocalStore = new LocalStoreMockBuilder()
            .clearQuiz(() => actualIsQuizCleared = true)
            .setQuiz(quiz => actualStoredQuiz = quiz)
            .setQuizDate(quizDate => actualStoredQuizDate = quizDate)
            .build();
        const mockFetchWrapper = new FetchWrapperMockBuilder()
            .fetch(async () => ({
                ok: true,
                json: async() => fetchedRawQuiz
            }))
            .build();
        const quizRepository = new QuizRepository({
            fetchWrapper: mockFetchWrapper,
            localStore: mockLocalStore
        });

        const actualQuiz = await quizRepository.loadLatestQuiz();

        assert.deepStrictEqual(actualQuiz, expectedQuiz);
        assert.deepStrictEqual(actualStoredQuiz, fetchedRawQuiz);
        assert.deepStrictEqual(actualStoredQuizDate, expectedStoredQuizDate);
        assert.strictEqual(actualIsQuizCleared, true);
    });
});
