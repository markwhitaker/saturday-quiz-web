import $ from "./jqueryModule.js";
import Presenter from "./Presenter.js";
import QuizRepository from "./QuizRepository.js";
import ScoreRepository from "./ScoreRepository.js";
import View from "./View.js";
import Test from "../js/test.js";

export default function start() {
    const test = Test.construct(100);
    console.log(test.getX());

    $(async () => {
        const presenter = new Presenter(
            new QuizRepository(),
            new ScoreRepository());
        const view = new View(presenter);
        await presenter.onViewReady(view);
    });
}
