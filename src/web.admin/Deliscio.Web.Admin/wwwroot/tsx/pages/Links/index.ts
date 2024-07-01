import { LinksPage } from './LinksPage';
import { LinksDetailsPage } from './LinkDetailsPage';

export default function Links() {
    if (document.querySelector('#links-list-container')) {
        const linksPage = new LinksPage();
    }
    else if (document.querySelector('#links-edit-container')) {
        const linkDetailsPage = new LinksDetailsPage();
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const page = Links();
});
