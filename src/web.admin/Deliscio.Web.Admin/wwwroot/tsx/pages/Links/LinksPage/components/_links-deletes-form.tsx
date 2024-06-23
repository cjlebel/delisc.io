import { getAntiForgeryToken } from '../../../../utils/dom/get-antiforgery-token';
import { initElement, ElementEvent } from '../../../../utils/dom/init-element';
import { toggleCheckboxes } from '../../../../utils/dom/toggle-checkboxes';
import LinksApis from '../apis';

/**
 * Handles the ability to delete multiple links on the List Index page
 */
export class DeleteLinksForm {
    formId = 'frmLinksDelete';

    form!: HTMLFormElement;

    btnSelectAll!: HTMLButtonElement;
    btnDeleteLinks!: HTMLButtonElement;

    checkBoxesToTrack: HTMLInputElement[] = [];

    onItemsDeletedFunc: Function | undefined;

    linksApis = new LinksApis();

    constructor(
        linkCheckBoxes: HTMLInputElement[] | null,
        onItemsDeleted?: Function | undefined
    ) {
        if (!document.getElementById(this.formId)) {
            console.error('The Delete form was not found');
            //TODO: ApplicationInsights.trackException(new Error('The Delete form was not found'));
            throw new Error('The Delete form was not found');
        }

        this.form = document.getElementById(this.formId) as HTMLFormElement;
        this.checkBoxesToTrack = linkCheckBoxes ?? [];
        this.onItemsDeletedFunc = onItemsDeleted;

        this.initForm();
    }

    /**
     * Updates the checkbox state of the checkbox that was clicked
     * @param checkBoxToUpdate The checkbox that was clicked from the caller
     */
    public updateCheckboxState = (checkBoxToUpdate: HTMLInputElement): void => {
        if (!checkBoxToUpdate) throw new Error('Checkbox to update was null or undefined');

        const checkboxFound = this.checkBoxesToTrack.find(
            (checkBox) => checkBox === checkBoxToUpdate
        );

        if (checkboxFound) checkboxFound.checked = checkBoxToUpdate.checked;

        if (this.checkBoxesToTrack.filter((checkBox) => checkBox.checked).length > 0) {
            this.btnDeleteLinks.disabled = false;
        } else {
            this.btnDeleteLinks.disabled = true;
        }
    };

    private handleSelectAllButtonClick = (e: Event) => {
        //this.checkBoxesToTrack = toggleAllCheckboxes(e, this.checkBoxesToTrack);
        toggleCheckboxes(e, this.checkBoxesToTrack);

        if (this.checkBoxesToTrack.find((checkBox) => checkBox.checked)) {
            this.btnDeleteLinks.disabled = false;
        } else {
            this.btnDeleteLinks.disabled = true;
        }
    };

    /**
     * Handles when the Delete button is clicked
     * @param e
     */
    private handleDeleteButtonClick = (e: Event) => {
        e.preventDefault();
        e.stopPropagation();

        const antiForgeryToken = getAntiForgeryToken(this.form);

        const linksToDelete = this.checkBoxesToTrack.filter((checkBox) => checkBox.checked) ?? [];

        let linkIds =
            (linksToDelete.map((link) => {
                if (link.dataset.id && link.dataset.id !== undefined)
                    return link.dataset.id;
            }) as string[]) ?? [];

        if (confirm('Are you sure you want to delete the selected links?')) {
            this.linksApis.deleteLinksAsync(
                linkIds,
                antiForgeryToken,
                this.onDeletesSuccess,
                this.onDeletesFailure
            );
        }
    };

    /**
     * Handles when a successful call to the delete api endpoint occurs
     * @param linkIds An array of link ids that were successfully deleted
     */
    private onDeletesSuccess = (linkIds: string[]) => {
        if (this.onItemsDeletedFunc && linkIds)
            this.onItemsDeletedFunc(linkIds);
    };

    /**
     * Handles when an error occurs when calling the delete api endpoint
     * @param error
     */
    private onDeletesFailure = (error: Error): void => {
        console.error(error);
        //ApplicationInsights.trackException(new Error(error));
        throw error;
    };

    private initForm(): void {
        this.btnSelectAll = initElement<HTMLButtonElement>(
            this.form,
            '#btnSelectLinks',
            { eventType: 'click', eventFunction: this.handleSelectAllButtonClick } as ElementEvent
        );

        this.btnDeleteLinks = initElement<HTMLButtonElement>(
            this.form,
            '#btnDeleteLinks',
            { eventType: 'click', eventFunction: this.handleDeleteButtonClick } as ElementEvent
        );

        this.btnDeleteLinks.disabled = true;
    }
}
