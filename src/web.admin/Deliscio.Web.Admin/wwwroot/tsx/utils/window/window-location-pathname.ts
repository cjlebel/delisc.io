/**
 * Gets the current path portion of the URL (excluding domain)
 * Example: 'https://www.stickeryou.com/products/tattoos/999' would return '/products/tattoos/999'
 * @returns
 */
export default function WindowPathname() {
    return window.location.pathname;
}