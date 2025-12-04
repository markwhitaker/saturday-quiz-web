import { describe, test, expect, mock } from 'bun:test';
import CalendarDate from "../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js";
import Quiz from "../../SaturdayQuizWeb/wwwroot/script/Quiz.js";
import QuizRepository from "../../SaturdayQuizWeb/wwwroot/script/QuizRepository.js";

describe('QuizRepository', () => {
    test('GIVEN quiz is cached WHEN load latest quiz THEN cached quiz is returned and quiz date is stored', () => {
        const cachedRawQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };
        const expectedQuiz = new Quiz(cachedRawQuiz);

        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(),
            setScores: mock()
        };
        const mockQuizCache = {
            getCachedQuiz: mock(() => cachedRawQuiz)
        };
        const quizRepository = new QuizRepository({
            localStore: mockLocalStore,
            quizCache: mockQuizCache
        });

        quizRepository.loadLatestQuiz().then(actualQuiz => {
                expect(actualQuiz).toEqual(expectedQuiz);
                expect(mockLocalStore.setQuizDate).toHaveBeenCalledWith(new CalendarDate(cachedRawQuiz.date));
            }
        );
    });

    test('GIVEN quiz is not cached WHEN load latest quiz THEN new quiz is fetched and quiz and quiz date are stored', async () => {
        const fetchedRawQuiz = {
            'date': '2021-02-03',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'fetched-question', 'answer': 'fetched-answer' }]
        };

        const expectedQuiz = new Quiz(fetchedRawQuiz);

        const mockFetchWrapper = {
            fetch: mock(async () => ({
                ok: true,
                json: async() => fetchedRawQuiz
            }))
        };
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(),
            setScores: mock()
        };
        const mockQuizCache = {
            getCachedQuiz: mock(() => undefined)
        };
        const quizRepository = new QuizRepository({
            fetchWrapper: mockFetchWrapper,
            localStore: mockLocalStore,
            quizCache: mockQuizCache
        });

        const actualQuiz = await quizRepository.loadLatestQuiz();

        expect(actualQuiz).toEqual(expectedQuiz);
        expect(mockLocalStore.setQuiz).toHaveBeenCalledWith(fetchedRawQuiz);
        expect(mockLocalStore.setQuizDate).toHaveBeenCalledWith(new CalendarDate(fetchedRawQuiz.date));
    });
});
