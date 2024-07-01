import { initElement } from '../../../utils/dom/init-element';
import { LinkEditForm } from './components/_link-edit-form';
import { LinkMetaPanel } from './components/_link-meta-panel';

import WindowReload from '../../../utils/window/window-reload';

/**
 * Responsible for managing the Links > Details page
 */
export default class LinksDetailsPage {
    editLinkForm!: LinkEditForm;
    messagePanel!: HTMLDivElement;
    metaPanel!: LinkMetaPanel;
    pageContainer!: HTMLDivElement;

    constructor() {
        if (!document.getElementById('links-edit-container')) {
            console.error('Unable to find the edit link page');
            //TODO: ApplicationInsights.trackException(new Error('Unable to find the page'));
            throw new Error('Unable to find the edit link page');
        };

        this.initForm();
    };

    private handleOnEditChanged = (response: { isSuccess: boolean, message: string, eventType: string }): void => {
        if (response.isSuccess) {
            //this.messagePanel.classList.add('success');
                if (confirm(response.message ?? 'Updated successfully'))
                    WindowReload();
        }
        else {
            alert(response.message ?? 'An error occurred');
        }
    };


    private initForm(): void {
        this.pageContainer = initElement<HTMLDivElement>(document, '#links-edit-container');

        this.editLinkForm = new LinkEditForm(this.handleOnEditChanged);

        this.messagePanel = initElement<HTMLDivElement>(this.pageContainer, '#message')

        this.metaPanel = new LinkMetaPanel();
    };
}
