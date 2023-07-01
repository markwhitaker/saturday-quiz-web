import $ from "./jqueryModule.js";
import Presenter from "./Presenter.js";
import QuizRepository from "../js/QuizRepository.js";
import ScoreRepository from "../js/ScoreRepository.js";
import View from "./View.js";

export default function start() {
    $(async () => {
        const presenter = new Presenter(
            new QuizRepository(),
            new ScoreRepository());
        const view = new View(presenter);
        await presenter.onViewReady(view);
    });
}
