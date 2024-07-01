/**
 * Initializes an element as type T with an optional event handler
 * @param parent The parent HTMLElement (not selector) that this element is a child of
 * @param selector The selector for the element that is to be initialized
 * @param eventHandler What to do when an event happens with this element
 * @returns The element as type T if defined. If not, then as plain ol' boring HTMLELement
 */
export function initElement<T extends HTMLElement>(
    parent: HTMLElement | Document,
    selector: string,
    elementEvent?: ElementEvent | undefined,
    callBack?: Function | undefined
): T {
    const element = parent.querySelector(selector) as T;

    if (!element) {
        console.log(`No elements found for selector: ${selector}`);
        throw new Error(`${selector} was not found`);
    }

    if (elementEvent)
        element.addEventListener(elementEvent.eventType, (e)=> elementEvent.eventFunction(e));

    if (callBack)
        callBack();

    return element;
}

/**
 * Initializes an array of elements as type T with an optional event handler
 * @param parent The parent HTMLElement (not selector) that this element is a child of
 * @param selector The selector for the element that is to be initialized
 * @param elementEvent What to do when an event happens with this element
 * @returns An array of elements as type T ifs defined. If not, then as plain ol' boring HTMLELement
 */
export function initElements<T extends HTMLElement>(
    parent: HTMLElement | Document,
    selector: string,
    elementEvent?: ElementEvent | undefined,
    callBack?: Function | undefined
): T[] {
    const elements = Array.from<T>(parent.querySelectorAll(selector));

    if (!elements) {
        console.log(`No elements found for selector: ${selector}`);
        throw new Error(`${selector} was not found`);
    }

    if (elementEvent)
        elements.forEach((element: T) => {
            element.addEventListener(elementEvent.eventType, (e) => elementEvent.eventFunction(e));

            if (callBack)
                callBack();
            });

    return elements;
}

export type ElementEvent = { eventType: string, eventFunction: Function };