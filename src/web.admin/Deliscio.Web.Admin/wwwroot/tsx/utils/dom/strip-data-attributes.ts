/**
 * Removes *** ALL *** of the data-* from the element
 * (could maybe also pass a collection of attributes to remove?)
 * @param element
 * @returns
 */
export default function StripDataAttributes(element: HTMLElement) {

    if (element) {

        const attributes = element.dataset as DOMStringMap;

        for (var attribute in attributes) {

            element.removeAttribute(`data-${attribute}`);

        }

    }

}