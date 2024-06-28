import { ApiDataResponse } from '@apis/api-response';
import { getAntiForgeryToken } from '@utils/dom/get-antiforgery-token';
import { initElement, initElements, ElementEvent } from '@utils/dom/init-element';
import { toggleCheckboxes } from '@utils/dom/toggle-checkboxes';
import WindowReload from '@utils/window/window-reload';

import { SearchLinksForm } from './components/_links-search-form';

import LinksApis from './apis';

/**
 * Responsible for managing the Links > Index page
 */
export default class LinksPage {
    pageContainer!: HTMLDivElement;

    searchLinksForm!: SearchLinksForm;

    deleteLinksForm!: HTMLFormElement;
    deleteLinksButton!: HTMLButtonElement;
    selectAllLinksButton!: HTMLButtonElement;

    linksTable!: HTMLTableElement;

    linkCheckBoxes: HTMLInputElement[] = [];

    linksApis = new LinksApis();

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
     */
    private handleOnSingleCheckBoxClicked = (e: Event) => {
        this.toggleDeleteButtonDisabledState();
    };

    /**
     * Handles when the SelectAll button is clicked
     * @param e
     */
    private handleOnSelectAllCheckboxesClicked = (e: Event) => {
        e.preventDefault();
        e.stopPropagation();

        toggleCheckboxes(e, this.linkCheckBoxes);

        this.toggleDeleteButtonDisabledState();
    }

    /**
     * Handles when the Delete button is clicked
     * @param e
     */
    private handleOnDeleteButtonClicked = (e: Event) => {
        e.preventDefault();
        e.stopPropagation();

        const antiForgeryToken = getAntiForgeryToken(this.deleteLinksForm);

        const linksToDelete = this.linkCheckBoxes.filter((checkBox) => checkBox.checked) ?? [];

        let linkIds =
            (linksToDelete.map((link) => {
                if (link.dataset.id && link.dataset.id !== undefined)
                    return link.dataset.id;
            }) as string[]) ?? [];

        if (confirm('Are you sure you want to delete the selected links?')) {
            this.linksApis.deleteLinksAsync(
                linkIds,
                antiForgeryToken,
                this.onDeleteLinksSuccess,
                this.onDeleteLinksFailure
            );
        }
    }

    /**
     * Handles when a successful call to the delete api endpoint occurs
     * @param linkIds An array of link ids that were successfully deleted
     */
    private onDeleteLinksSuccess = (response: ApiDataResponse<string[]>) => {
        if (response.isSuccess) {
            if (response.data) {
                const linkIds = response.data;
                const linkIdsAsString = linkIds.length === 1 ? linkIds[0] : linkIds.join(', ');

                alert(`The following links were deleted: ${linkIdsAsString}`);

                WindowReload();
            }
            else {
                alert('It seems that no links were deleted');
            }
        }
        else {
            alert(response.message ?? 'An error occurred while deleting the links');
        }
    };

    /**
     * Handles when an error occurs when calling the delete api endpoint
     * @param error
     */
    private onDeleteLinksFailure = (error: Error): void => {
        console.error(error);
        //ApplicationInsights.trackException(new Error(error));
        throw error;
    };

    /**
     * Sets the disabled state of the delete button based on whether 
     * there is at least one checkbox selected or not
     */
    private toggleDeleteButtonDisabledState(): void {
        this.deleteLinksButton.disabled =
            (this.linkCheckBoxes.find((checkBox) => checkBox.checked) ?? null) === null;
    };

    /**
     * Handles seltting up the SelectAll and Delete buttons
     */
    private initDeleteLinksForm(): void {
        const deleteFormId = 'frmLinksDelete';
        this.deleteLinksForm = initElement<HTMLFormElement>(document, `#${deleteFormId}`)!;
        this.deleteLinksButton = this.deleteLinksButton = initElement<HTMLButtonElement>(
            this.deleteLinksForm,
            '#btnDeleteLinks',
            { eventType: 'click', eventFunction: this.handleOnDeleteButtonClicked } as ElementEvent
        );

        this.toggleDeleteButtonDisabledState();

        this.selectAllLinksButton = initElement<HTMLButtonElement>(
            this.deleteLinksForm,
            '#btnSelectLinks',
            { eventType: 'click', eventFunction: this.handleOnSelectAllCheckboxesClicked } as ElementEvent
        );
    }

    private initForm(): void {
        this.pageContainer = initElement<HTMLDivElement>(document, '#links-list-container');

        /* Search Links Form */
        this.searchLinksForm = new SearchLinksForm();

        /* Links Table */
        this.linksTable = initElement<HTMLTableElement>(this.pageContainer, '#links-table');

        this.linkCheckBoxes = initElements<HTMLInputElement>(
            this.linksTable,
            'input[name="selected-link"]',
            {
                eventType: 'click',
                eventFunction: this.handleOnSingleCheckBoxClicked
            } as ElementEvent);

        /* Delete Links Form */
        this.initDeleteLinksForm();
    }
}
