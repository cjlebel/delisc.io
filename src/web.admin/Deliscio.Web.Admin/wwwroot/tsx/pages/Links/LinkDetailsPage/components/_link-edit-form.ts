import { initElement } from '../../../../utils/dom/init-element';
import { getAntiForgeryToken } from '@utils/dom/get-antiforgery-token';
import { validateForm } from '@utils/dom/validate-form';

import LinkApis from '../apis';
import { EditLinkRequest } from '../apis/editLinkRequest';

type EditLinkEventTypes = 'Activate' | 'Deactivate' | 'Cancel' | 'Delete' | 'Undelete' | 'Save';

export class LinkEditForm {
    formId = 'frmEditLink';

    linkId: string = '';
    isActive: boolean = false;
    isDeleted: boolean = false;
    isFlagged: boolean = false;

    // Elements
    // Note: Because I'm checking/throwing errors if the elements are not found,
    // I'm using the non-null assertion operator (!) to say that they will not be null.
    frmEditLink!: HTMLFormElement;

    txtLinkTitle!: HTMLInputElement;
    txtLinkDescription!: HTMLInputElement;
    txtLinkDomain!: HTMLInputElement;
    txtLinkTags!: HTMLInputElement;
    txtLinkUrl!: HTMLInputElement;

    btnSave!: HTMLButtonElement;
    btnActivate!: HTMLButtonElement;
    btnDelete!: HTMLButtonElement;
    btnCancel!: HTMLButtonElement;

    onEditChangedCallback?: Function | null;

    linkApis = new LinkApis();

    constructor(onEditChange?: Function | undefined) {
        this.initForm(onEditChange);
    }


    /**
     *  Sets the state of the Activate/Deactivate button based on the value.
     * If the link is NOT active, the button will say "Activate" and be a primary button.
     * If the link is active, the button will say "Deactivate" and be a secondary button.
     * @param isLinkActive Whether or not the link is active.
     */
    private setActiveButtonState = (isLinkActive: boolean) => {
        if (!isLinkActive) {
            this.btnActivate.textContent = 'Activate';
            this.btnActivate.classList.remove('btn-secondary');
        }
        else {
            this.btnActivate.textContent = 'Deactivate';
            this.btnActivate.classList.add('btn-secondary');
        }
    }

    private handleOnActivateClick = (e: Event) => {
        const isNowActive = !this.isActive;

        this.isActive = isNowActive;

        this.setActiveButtonState(isNowActive);
    };


    private setDeleteButtonState = (isLinkDeleted: boolean) => {
        if (!isLinkDeleted) {
            this.btnDelete.textContent = 'Delete';
            this.btnDelete.classList.remove('btn-secondary');
            this.btnDelete.classList.add('btn-danger');
        }
        else {
            this.btnDelete.textContent = 'UnDelete';
            this.btnDelete.classList.remove('btn-danger');
            this.btnDelete.classList.add('btn-secondary');
        }
    }

    /**
     * Handles both Delete and Undelete actions.
     * @param e
     */
    private handleOnDeleteClick = (e: Event) => {
        e.preventDefault();
        e.stopPropagation();

        const isNowDeleted = !this.isDeleted;

        const antiForgeryToken = getAntiForgeryToken(this.frmEditLink);

        this.isDeleted = isNowDeleted;

        if (isNowDeleted)
            this.linkApis.deleteLinkAsync(this.linkId, antiForgeryToken, this.handleOnDeleteSuccess, this.handleOnDeleteFail);
        else
            this.linkApis.unDeleteLinkAsync(this.linkId, antiForgeryToken, this.handleOnUnDeleteSuccess, this.handleOnUnDeleteFail);
    };

    /**
     * Handles when the DELETE is successful
     * @param response
     */
    private handleOnDeleteSuccess = (response: { isSuccess: boolean; message: string }) => {
        if (response.isSuccess) {
            this.isDeleted = false;
            this.setDeleteButtonState(this.isDeleted);

            this.handleOnEditChanged( true, response.message, 'Delete');
        };
    };

    /**
     * Handles when the DELETE fails
     * @param error
     */
    private handleOnDeleteFail = (error: Error) => {
        console.error(error ?? 'An error occurred while trying to undelete the link');
        this.handleOnEditChanged(false, error.toString(), 'Delete');
    };

    /**
     * Handles when the UNDELETE is successful
     * @param response
     */
    private handleOnUnDeleteSuccess = (response: { isSuccess: boolean; message: string }) => {
        if (response.isSuccess) {
            this.isDeleted = false;
            this.setDeleteButtonState(this.isDeleted);

            this.handleOnEditChanged(true, response.message, 'Undelete');
        }
    };

    /**
     * Handles when the UNDELETE fails
     * @param error
     */
    private handleOnUnDeleteFail = (error: Error) => {
        console.error(error ?? 'An error occurred while trying to undelete the link');

        this.handleOnEditChanged(false, error.toString(), 'Undelete');
    };

    private handleOnSaveClick = (e: Event) => {
        e.preventDefault();
        e.stopPropagation();

        const isFormValidated = validateForm(this.frmEditLink.elements)

        if (!isFormValidated?.isSuccess) {
            //this.displayMessage(isFormValidated?.isSuccess, isFormValidated?.message);

            return;
        }

        const antiForgeryToken = getAntiForgeryToken(this.frmEditLink);

        const link: EditLinkRequest = {
            linkId: this.linkId,
            title: this.txtLinkTitle.value,
            description: this.txtLinkDescription.value,
            domain: this.txtLinkDomain.value,
            tags: this.txtLinkTags.value ? this.txtLinkTags.value.split(',').map(function (i) {
                return i.trim();
            }) : []
        };

        this.linkApis.updateLinkAsync(link, antiForgeryToken, this.handleOnSubmitLinkSuccess, this.handleOnSubmitLinkFail);
    }

    private handleOnSubmitLinkSuccess = (response: { isSuccess: boolean; message: string }) => {
        this.handleOnEditChanged(response.isSuccess, response.message, 'Undelete');
    }

    private handleOnSubmitLinkFail = (error: Error) => {
        this.handleOnEditChanged(false, error.message ?? 'And error occurred while trying to Save the link', 'Save');
    }

    private handleOnEditChanged = (isSuccess: boolean, message: string, eventType: EditLinkEventTypes) => {
        if (this.onEditChangedCallback)
            this.onEditChangedCallback({ isSuccess, message, eventType });

    };

    private handleOnCancelClick = (e: Event) => {

    }

    /**
     * Initializes the elements on the Edit form. 
     * @param onEditChanged
     */
    private initForm(onEditChanged: Function | undefined): void {

        this.frmEditLink = initElement<HTMLFormElement>(document, `#${this.formId}`)!;

        if (!this.frmEditLink.getAttribute('data-link-id')) {
            console.log('The form does not have a data-link-id attribute');
            throw new Error('The form does not have a data-link-id attribute');
        }
        else {
            this.linkId = this.frmEditLink.getAttribute('data-link-id')!;
        }

        this.isActive = this.frmEditLink.getAttribute('data-isactive') === 'true';
        this.isDeleted = this.frmEditLink.getAttribute('data-isdeleted') === 'true';
        this.isFlagged = this.frmEditLink.getAttribute('data-isflagged') === 'true';

        this.txtLinkTitle = initElement<HTMLInputElement>(this.frmEditLink, '#title')!;
        this.txtLinkDescription = initElement<HTMLInputElement>(this.frmEditLink, '#description')!;
        this.txtLinkDomain = initElement<HTMLInputElement>(this.frmEditLink, '#domain')!;
        this.txtLinkTags = initElement<HTMLInputElement>(this.frmEditLink, '#tags')!;
        this.txtLinkUrl = initElement<HTMLInputElement>(this.frmEditLink, '#url')!;

        this.btnActivate = initElement<HTMLButtonElement>(this.frmEditLink, '#btnActivate',
            { eventType: 'click', eventFunction: this.handleOnActivateClick })!;
        this.setActiveButtonState(this.isActive);

        this.btnDelete = initElement<HTMLButtonElement>(this.frmEditLink, '#btnDelete',
            { eventType: 'click', eventFunction: this.handleOnDeleteClick })!;
        this.setDeleteButtonState(this.isDeleted);


        this.btnCancel = initElement<HTMLButtonElement>(this.frmEditLink, '#btnCancel',
            { eventType: 'click', eventFunction: this.handleOnCancelClick })!;

        this.btnSave = initElement<HTMLButtonElement>(this.frmEditLink, '#btnSave',
            { eventType: 'click', eventFunction: this.handleOnSaveClick })!;

        this.onEditChangedCallback = onEditChanged ?? null;
    }
}