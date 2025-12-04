import { describe, test, expect, mock } from 'bun:test';
import QuestionScore from "../../SaturdayQuizWeb/wwwroot/script/QuestionScore.js";
import ScoreRepository from '../../SaturdayQuizWeb/wwwroot/script/ScoreRepository.js';

describe('ScoreRepository', () => {
    test('GIVEN no scores WHEN total score is calculated THEN total score is zero', () => {
        const scoreRepository = new ScoreRepository();
        expect(scoreRepository.getTotalScore()).toBe(0);
    });

    test('GIVEN no scores are stored WHEN repository is initialised THEN new score array is stored', () => {
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => undefined),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });

        scoreRepository.initialiseScores({ getQuestions: () => [{}, {}, {}] });

        expect(mockLocalStore.setScores).toHaveBeenCalledWith('0,0,0');
    });

    test('GIVEN scores are stored WHEN repository is initialised THEN existing score array is stored', () => {
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => '0,0.5,1'),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });

        scoreRepository.initialiseScores({ getQuestions: () => [{}, {}, {}] });

        expect(mockLocalStore.setScores).toHaveBeenCalledWith('0,0.5,1');
    });

    test('GIVEN scores are stored WHEN total score is calculated THEN expected total score is returned', () => {
        const expectedTotalScore = 1.5;
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => '0,0.5,1'),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });
        scoreRepository.initialiseScores();

        const actualTotalScore = scoreRepository.getTotalScore();

        expect(actualTotalScore).toBe(expectedTotalScore);
    });

    test('GIVEN scores are stored WHEN all scores are retrieved THEN expected scores are returned', () => {
        const expectedAllScores = [QuestionScore.NONE, QuestionScore.HALF, QuestionScore.FULL];
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => '0,0.5,1'),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });
        scoreRepository.initialiseScores();

        const actualAllScores = scoreRepository.getAllScores();

        expect(actualAllScores).toEqual(expectedAllScores);
    });

    test('GIVEN all stored scores are zero WHEN has scores THEN false is returned', () => {
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => '0,0,0'),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });
        scoreRepository.initialiseScores();

        const actualHasScores = scoreRepository.hasScores();

        expect(actualHasScores).toBe(false);
    });

    test('GIVEN one stored score is greater than zero WHEN has scores THEN true is returned', () => {
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => '0,0,0.5'),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });
        scoreRepository.initialiseScores();

        const actualHasScores = scoreRepository.hasScores();

        expect(actualHasScores).toBe(true);
    });

    test('GIVEN scores are stored WHEN question score is retrieved THEN expected question score is returned', () => {
        const expectedQuestionScore = 0.5;
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => '0,0.5,1'),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });
        scoreRepository.initialiseScores();

        const actualQuestionScore = scoreRepository.getScore(2);

        expect(actualQuestionScore).toBe(expectedQuestionScore);
    });

    test('GIVEN scores are stored WHEN question score is set THEN expected scores are stored', () => {
        const mockLocalStore = {
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            getQuizDate: mock(),
            setQuizDate: mock(),
            getQuiz: mock(),
            setQuiz: mock(),
            clearQuiz: mock(),
            getScores: mock(() => '0,0,0'),
            setScores: mock()
        };
        const scoreRepository = new ScoreRepository({
            localStore: mockLocalStore
        });
        scoreRepository.initialiseScores();

        scoreRepository.setScore(3, QuestionScore.FULL);

        expect(mockLocalStore.setScores).toHaveBeenCalledWith('0,0,1');
    });});
