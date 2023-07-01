export default class QuizMetadata {
    private readonly _id: string;
    private readonly _date: Date;
    private readonly _title: string;
    private readonly _url: string;
    private readonly _source: string;

    constructor(jsonObject) {
        this._id = jsonObject.id;
        this._date = new Date(jsonObject.date);
        this._title = jsonObject.title;
        this._url = jsonObject.url;
        this._source = jsonObject.source;
    }

    get id(): string {
        return this._id;
    }

    get date(): Date {
        return this._date;
    }

    get title(): string {
        return this._title;
    }

    get url(): string {
        return this._url;
    }

    get source(): string {
        return this._source;
    }
}
