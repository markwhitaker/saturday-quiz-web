export default class NavigatorWrapper {
    get isShareSupported() {
        return navigator && navigator.share;
    }

    async share(data) {
        await navigator.share(data);
    }
}
