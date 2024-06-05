export default class NavigatorWrapper {
    get isShareSupported() {
        return navigator.share !== undefined;
    }

    async share(data) {
        await navigator.share(data);
    }
}
