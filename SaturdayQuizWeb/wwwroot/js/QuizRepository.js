var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import Quiz from "./Quiz.js";
class QuizRepository {
    loadLatestQuiz() {
        return __awaiter(this, void 0, void 0, function* () {
            const response = yield fetch(QuizRepository.QUIZ_ENDPOINT);
            if (!response.ok) {
                throw new Error(`Failed to fetch ${(QuizRepository.QUIZ_ENDPOINT)}. ${response.status}: ${response.statusText}`);
            }
            const quizJson = yield response.json();
            return new Quiz(quizJson);
        });
    }
}
QuizRepository.QUIZ_ENDPOINT = "api/quiz";
export default QuizRepository;
//# sourceMappingURL=QuizRepository.js.map