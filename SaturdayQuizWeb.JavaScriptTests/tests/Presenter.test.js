import {expect, test} from '@jest/globals';
import Presenter from "../../SaturdayQuizWeb/wwwroot/script/Presenter.js";

test('Presenter should be defined', () => {
    const presenter = new Presenter();
    expect(presenter).toBeDefined();
});
