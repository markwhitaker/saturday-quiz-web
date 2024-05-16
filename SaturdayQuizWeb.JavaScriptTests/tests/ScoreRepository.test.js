import {beforeEach, expect, jest, test} from '@jest/globals';
import LocalStore from "../../SaturdayQuizWeb/wwwroot/script/LocalStore.js";
import ScoreRepository from "../../SaturdayQuizWeb/wwwroot/script/ScoreRepository.js";
jest.mock("../../SaturdayQuizWeb/wwwroot/script/LocalStore.js");

beforeEach(() => {
//    LocalStore.mockClear();
});

test("ScoreRepository should be defined", () => {
    const scoreRepository = new ScoreRepository();
    expect(scoreRepository).toBeDefined();
//    expect(scoreRepository.allScores).toBe([])
});

test("GIVEN no scores are saved WHEN ", () => {
    // Given
    /*const scoresMock = */jest
        .spyOn(LocalStore.prototype, "scores", "get")
        .mockReturnValue("0,0.5,1");

    // When
    const scoreRepository = new ScoreRepository(LocalStore);
    scoreRepository.initialiseScores();
    const allScores = scoreRepository.allScores;

    // Then
    expect(allScores).toBe([0,0.5,1]);
});
