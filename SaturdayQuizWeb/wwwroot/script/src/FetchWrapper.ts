export default class FetchWrapper {
    async fetch(url: string): Promise<Response> {
        return await fetch(url);
    }
}
