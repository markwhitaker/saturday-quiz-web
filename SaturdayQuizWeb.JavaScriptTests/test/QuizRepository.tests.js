import { suite, test } from 'mocha';
import assert from 'assert';
import CalendarDate from "../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js";
import MockFetchWrapperBuilder from "./mocks/MockFetchWrapperBuilder.js";
import MockLocalStoreBuilder from "./mocks/MockLocalStoreBuilder.js";
import MockQuizCacheBuilder from "./mocks/MockQuizCacheBuilder.js";
import Quiz from "../../SaturdayQuizWeb/wwwroot/script/Quiz.js";
import QuizRepository from "../../SaturdayQuizWeb/wwwroot/script/QuizRepository.js";

suite('QuizRepository', () => {
    test('GIVEN quiz is cached WHEN load latest quiz THEN cached quiz is returned and quiz date is stored', () => {
        const cachedRawQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };
        const expectedQuiz = new Quiz(cachedRawQuiz);

        let actualStoredQuizDate;

        const mockLocalStore = new MockLocalStoreBuilder()
            .setQuizDate(date => actualStoredQuizDate = date)
            .build();
        const mockQuizCache = new MockQuizCacheBuilder()
            .getCachedQuiz(() => cachedRawQuiz)
            .build();
        const quizRepository = new QuizRepository({
            localStore: mockLocalStore,
            quizCache: mockQuizCache
        });

        quizRepository.loadLatestQuiz().then(actualQuiz => {
                assert.deepStrictEqual(actualQuiz, expectedQuiz);
                assert.deepStrictEqual(actualStoredQuizDate, new CalendarDate(cachedRawQuiz.date));
            }
        );
    });

    test('GIVEN quiz is not cached WHEN load latest quiz THEN new quiz is fetched and quiz and quiz date are stored', async () => {
        const fetchedRawQuiz = {
            'date': '2021-02-03',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'fetched-question', 'answer': 'fetched-answer' }]
        };

        const expectedQuiz = new Quiz(fetchedRawQuiz);

        let actualStoredQuiz;
        let actualStoredQuizDate;

        const mockFetchWrapper = new MockFetchWrapperBuilder()
            .fetch(async () => ({
                ok: true,
                json: async() => fetchedRawQuiz
            }))
            .build();
        const mockLocalStore = new MockLocalStoreBuilder()
            .setQuiz(quiz => actualStoredQuiz = quiz)
            .setQuizDate(date => actualStoredQuizDate = date)
            .build();
        const mockQuizCache = new MockQuizCacheBuilder()
            .getCachedQuiz(() => undefined)
            .build();
        const quizRepository = new QuizRepository({
            fetchWrapper: mockFetchWrapper,
            localStore: mockLocalStore,
            quizCache: mockQuizCache
        });

        const actualQuiz = await quizRepository.loadLatestQuiz();

        assert.deepStrictEqual(actualQuiz, expectedQuiz);
        assert.deepStrictEqual(actualStoredQuiz, fetchedRawQuiz);
        assert.deepStrictEqual(actualStoredQuizDate, new CalendarDate(fetchedRawQuiz.date));
    });
});
