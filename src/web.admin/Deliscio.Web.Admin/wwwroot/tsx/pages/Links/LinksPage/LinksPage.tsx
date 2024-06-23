import { DeleteLinksForm } from './components/_links-deletes-form';
import { SearchLinksForm } from './components/_links-search-form';

import { initElement, initElements, ElementEvent } from '@utils/dom/init-element';

import WindowReload from '@utils/window/window-reload';

/**
 * Responsible for managing the Links > Index page
 */
export default class LinksPage {
    pageContainer!: HTMLDivElement;

    searchLinksForm!: SearchLinksForm;
    deleteLinksForm!: DeleteLinksForm;

    linksTable!: HTMLTableElement;

    selectLinkBoxes: HTMLInputElement[] = [];

    constructor() {
        if (!document.querySelector('#links-list-container')) {
            console.error('Unable to find the page');
            //TODO: ApplicationInsights.trackException(new Error('Unable to find the page'));
            throw new Error('Unable to find the page');
        }

        this.initForm();
    }

    /**
     * Handles when an individual checkbox is clicked.
     * Sets its value to true or false
     * Then notifies the DeleteLinksForm
     * @param e
     */
    private handleCheckBoxClick = (e: Event) => {
        const chk = e.target as HTMLInputElement;
        chk.value = chk.checked ? 'true' : 'false';

        this.deleteLinksForm.updateCheckboxState(e.target as HTMLInputElement);
    };

    /**
     * Handles when items are deleted
     * @param ids
     */
    private onItemsDeletedCallback(ids: string[] | null): void {
        console.log(`The following links were deleted: ${ids}`);

        WindowReload();
    }

    private initForm(): void {
        this.pageContainer = initElement<HTMLDivElement>(document, '#links-list-container');

        this.linksTable = initElement<HTMLTableElement>(this.pageContainer, '#links-table');

        //TODO: Have class for Table and get checkboxes from it.
        //this.checkBoxes = [
        //    ...this.linksTable.querySelectorAll('input[type="checkbox"]'),
        //] as HTMLInputElement[];

        this.selectLinkBoxes = initElements<HTMLInputElement>(this.linksTable, 'input[name="selected-link"]', { eventType: 'click', eventFunction: this.handleCheckBoxClick } as ElementEvent);

        this.deleteLinksForm = new DeleteLinksForm(this.selectLinkBoxes, this.onItemsDeletedCallback);

        this.searchLinksForm = new SearchLinksForm();
    }
}
