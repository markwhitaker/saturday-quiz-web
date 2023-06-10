export default class QuizMetadata {
    constructor(jsonObject) {
        this._id = jsonObject.id;
        this._date = new Date(jsonObject.date);
        this._title = jsonObject.title;
        this._url = jsonObject.url;
        this._source = jsonObject.source;
    }
    get id() {
        return this._id;
    }
    get date() {
        return this._date;
    }
    get title() {
        return this._title;
    }
    get url() {
        return this._url;
    }
    get source() {
        return this._source;
    }
}
//# sourceMappingURL=QuizMetadata.js.map