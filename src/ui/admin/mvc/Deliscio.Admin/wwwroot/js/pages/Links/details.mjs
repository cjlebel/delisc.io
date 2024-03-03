import DeleteLink from './components/delete-link.mjs';
import EditLink from './components/edit-link.mjs';

// TODO: Move form actions to here, and leave the implementaions in the components
export default function LinkDetails() {
    const frmEdit = document.querySelector('#links-edit-container form');

    const deleteLink = new DeleteLink();
    const editLink = new EditLink();
}

const editLinksInstance = new LinkDetails();