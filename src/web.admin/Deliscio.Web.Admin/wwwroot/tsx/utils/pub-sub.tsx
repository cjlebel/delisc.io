/**
 * For some reason this file is getting compiled as its own mjs file. Should not be happening.
 */
type Callback = (...args: any[]) => void;

/**
 * A simple PubSub that allows for sublishing and subscribing across modules.
 */
class PubSub {
    private eventListeners: { [eventName: string]: Callback[] } = {};

    subscribe(eventName: EventNames, callback: Callback) {
        if (!this.eventListeners[eventName]) {
            this.eventListeners[eventName] = [];
        }
        this.eventListeners[eventName].push(callback);
    }

    publish(eventName: EventNames, ...args: any[]) {
        if (this.eventListeners[eventName]) {
            this.eventListeners[eventName].forEach(callback => callback(...args));
        }
    }
}

export const PubSubHub = new PubSub();

export const PubSubEvents = {
    CURRENCY_CHANGED: 'currencyChanged',
    CART_UPDATED: 'cartUpdated',
    USER_LOGGED_IN: 'userLoggedIn',
} as const;

export type EventNames = typeof PubSubEvents[keyof typeof PubSubEvents];