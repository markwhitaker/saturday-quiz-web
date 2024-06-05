import { suite, test } from 'mocha';
import assert from 'assert';
import LocalStoreMockBuilder from "./mocks/LocalStoreMockBuilder.js";
import QuestionScore from "../../SaturdayQuizWeb/wwwroot/script/QuestionScore.js";
import ScoreRepository from '../../SaturdayQuizWeb/wwwroot/script/ScoreRepository.js';

suite('ScoreRepository', function() {
    test('GIVEN no scores WHEN total score is calculated THEN total score is zero', () => {
        const scoreRepository = new ScoreRepository();
        assert.strictEqual(scoreRepository.totalScore, 0);
    });

    test('GIVEN no scores are stored WHEN repository is initialised THEN new score array is stored', () => {
        let actualStoredScores = undefined;
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => undefined)
            .setScores(value => actualStoredScores = value)
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);

        scoreRepository.initialiseScores({ questions: [{}, {}, {}] });

        assert.strictEqual(actualStoredScores, '0,0,0');
    });

    test('GIVEN scores are stored WHEN repository is initialised THEN existing score array is stored', () => {
        let actualStoredScores = undefined;
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => '0,0.5,1')
            .setScores(value => actualStoredScores = value)
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);

        scoreRepository.initialiseScores({ questions: [{}, {}, {}] });

        assert.strictEqual(actualStoredScores, '0,0.5,1');
    });

    test('GIVEN scores are stored WHEN total score is calculated THEN expected total score is returned', () => {
        const expectedTotalScore = 1.5;
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => '0,0.5,1')
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);
        scoreRepository.initialiseScores();

        const actualTotalScore = scoreRepository.totalScore;

        assert.strictEqual(actualTotalScore, expectedTotalScore);
    });

    test('GIVEN scores are stored WHEN all scores are retrieved THEN expected scores are returned', () => {
        const expectedAllScores = [QuestionScore.NONE, QuestionScore.HALF, QuestionScore.FULL];
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => '0,0.5,1')
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);
        scoreRepository.initialiseScores();

        const actualAllScores = scoreRepository.allScores;

        assert.deepStrictEqual(actualAllScores, expectedAllScores);
    });

    test('GIVEN all stored scores are zero WHEN has scores THEN false is returned', () => {
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => '0,0,0')
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);
        scoreRepository.initialiseScores();

        const actualHasScores = scoreRepository.hasScores;

        assert.strictEqual(actualHasScores, false);
    });

    test('GIVEN one stored score is greater than zero WHEN has scores THEN true is returned', () => {
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => '0,0,0.5')
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);
        scoreRepository.initialiseScores();

        const actualHasScores = scoreRepository.hasScores;

        assert.strictEqual(actualHasScores, true);
    });

    test('GIVEN scores are stored WHEN question score is retrieved THEN expected question score is returned', () => {
        const expectedQuestionScore = 0.5;
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => '0,0.5,1')
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);
        scoreRepository.initialiseScores();

        const actualQuestionScore = scoreRepository.getScore(2);

        assert.strictEqual(actualQuestionScore, expectedQuestionScore);
    });

    test('GIVEN scores are stored WHEN question score is set THEN expected scores are stored', () => {
        const expectedStoredScores = '0,0,1';
        let actualStoredScores = '';
        const mockLocalStore = new LocalStoreMockBuilder()
            .getScores(() => '0,0,0')
            .setScores(value => actualStoredScores = value)
            .build();
        const scoreRepository = new ScoreRepository(mockLocalStore);
        scoreRepository.initialiseScores();

        scoreRepository.setScore(3, QuestionScore.FULL);

        assert.strictEqual(actualStoredScores, expectedStoredScores);
    });});
