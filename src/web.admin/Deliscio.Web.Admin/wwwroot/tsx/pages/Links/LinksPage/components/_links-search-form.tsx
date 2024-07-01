import { initElement, ElementEvent } from '@utils/dom/init-element';

export class SearchLinksForm {
    formId = 'frmLinksSearch';

    // Elements
    // Note: Because I'm checking/throwing errors if the elements are not found,
    // I'm using the non-null assertion operator (!) to say that they will not be null.
    frmSearchLinks!: HTMLFormElement;

    chkSearchIsActive!: HTMLInputElement;
    chkSearchIsDeleted!: HTMLInputElement;
    chkSearchIsFlagged!: HTMLInputElement;

    txtSearchDomain!: HTMLInputElement;
    txtSearchText!: HTMLInputElement;
    txtSearchTags!: HTMLInputElement;

    btnSearch!: HTMLButtonElement;
    btnClear!: HTMLButtonElement;

    constructor() {
        this.initForm();
    }

    /**
     * Initialize the elements in the search form
     */
    private initForm(): void {
        this.frmSearchLinks = initElement<HTMLFormElement>(document, `#${this.formId}`)!;

        // Is Active checkbox
        this.chkSearchIsActive = initElement<HTMLInputElement>(
            this.frmSearchLinks,
            '#chk-is-active',
            { eventType: 'change', eventFunction: this.onChkChanged } as ElementEvent
        );
        this.chkSearchIsActive.value = this.chkSearchIsActive.checked ? 'true' : 'false';

        // Is Deleted checkbox
        this.chkSearchIsDeleted = initElement<HTMLInputElement>(
            this.frmSearchLinks,
            '#chk-is-deleted',
            { eventType: 'change', eventFunction: this.onChkChanged } as ElementEvent
        );
        this.chkSearchIsDeleted.value = this.chkSearchIsDeleted.checked ? 'true' : 'false';

        // Is Flagged checkbox
        this.chkSearchIsFlagged = initElement<HTMLInputElement>(
            this.frmSearchLinks,
            '#chk-is-flagged',
            { eventType: 'change', eventFunction: this.onChkChanged } as ElementEvent
        );
        this.chkSearchIsFlagged.value = this.chkSearchIsFlagged.checked ? 'true' : 'false';


        this.txtSearchDomain = initElement<HTMLInputElement>(this.frmSearchLinks, '#txt-domain')!;

        this.txtSearchText = initElement<HTMLInputElement>(this.frmSearchLinks, '#txt-term')!;

        this.txtSearchTags = initElement<HTMLInputElement>(this.frmSearchLinks, ' #txt-tags')!;

        this.btnSearch = initElement<HTMLButtonElement>(
            this.frmSearchLinks,
            '#btn-search',
            { eventType: 'click', eventFunction: this.handleSearchButtonClick } as ElementEvent
        )!;

        this.btnClear = initElement<HTMLButtonElement>(
            this.frmSearchLinks,
            '#btn-clear',
            { eventType: 'click', eventFunction: this.handleClearButtonClick } as ElementEvent
        )!;
    }

    /**
     * Handles when any of the checkboxes value changes.
     * @param e The Event
     */
    private onChkChanged = (e: Event) => {
        const target = e.target as HTMLInputElement | null;
        if (target) {
            target.value = target.checked ? 'true' : 'false';
        }
    };

    /**
     * Handles when the Search button was clicked.
     * This does NOT post the form, but instead constructs the URL and redirects the user to it.
     * @param e The Event
     */
    handleSearchButtonClick = (e: Event) => {
        e.preventDefault();
        e.stopPropagation();

        const url = `/links?term=${this.txtSearchText.value}&tags=${this.txtSearchTags.value}&domain=${this.txtSearchDomain.value}&isActive=${this.chkSearchIsActive.value}&isFlagged=${this.chkSearchIsFlagged.value}&isDeleted=${this.chkSearchIsDeleted.value}`;

        window.location.href = url;
    };

    handleClearButtonClick = (e: Event) => {
        const url = '/links';

        window.location.href = url;
    }
}
