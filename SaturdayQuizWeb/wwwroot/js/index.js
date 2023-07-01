var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import $ from "./jqueryModule.js";
import Presenter from "./Presenter.js";
import QuizRepository from "./QuizRepository.js";
import ScoreRepository from "./ScoreRepository.js";
import View from "./View.js";
export default function start() {
    $(() => __awaiter(this, void 0, void 0, function* () {
        const presenter = new Presenter(new QuizRepository(), new ScoreRepository());
        const view = new View(presenter);
        yield presenter.onViewReady(view);
    }));
}
//# sourceMappingURL=index.js.map