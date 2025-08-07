import "https://code.jquery.com/jquery-3.7.1.min.js";

declare const window: Window & {
    jQuery: JQueryStatic;
};

const $: JQueryStatic = window.jQuery.noConflict(true);

export default $;
