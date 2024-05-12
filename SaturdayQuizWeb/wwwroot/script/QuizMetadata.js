import CalendarDate from "./CalendarDate.js";

export default class QuizMetadata {
    constructor(object) {
        this.id = object.id;
        this.date = new CalendarDate(object.date);
        this.title = object.title;
        this.url = object.url;
        this.source = object.source;
    }
}
