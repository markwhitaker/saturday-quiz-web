import $ from "./jqueryModule.js";
import Presenter from "./Presenter.js";
import View from "./View.js";

export default function start() {
    $(async () => {
        const presenter = new Presenter();
        const view = new View(presenter);
        await presenter.onViewReady(view);
    });
}
