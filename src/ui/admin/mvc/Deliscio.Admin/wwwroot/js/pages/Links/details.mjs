/**
 * Responsible for managing the Links > Details page
 */
import DeleteLink from './components/link-delete.mjs';
import EditLink from './components/link-edit.mjs';

// TODO: Move form actions to here, and leave the implementaions in the components
export default function LinkDetails() {
    const frmEdit = document.querySelector('#links-edit-container form');

    const deleteLink = new DeleteLink();
    const editLink = new EditLink();
}

const linksDetailsInstance = new LinkDetails();