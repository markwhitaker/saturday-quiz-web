export default class NavigatorWrapper {
    isShareSupported() {
        return navigator && navigator.share;
    }

    async share(data) {
        await navigator.share(data);
    }
}
