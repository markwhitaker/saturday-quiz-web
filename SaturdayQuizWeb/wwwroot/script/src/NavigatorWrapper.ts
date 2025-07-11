export default class NavigatorWrapper {
    get isShareSupported(): boolean {
        return navigator != null && 'share' in navigator;
    }

    async share(data: ShareData): Promise<void> {
        await navigator.share(data);
    }
}
