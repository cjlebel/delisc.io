/**
 * Responsible for managing the Links > Index page
 */
import DeleteLinks from "./components/links-delete.mjs";
import SearchLinks from "./components/links-search.mjs";
export default function LinksIndex() {
    const deleteLinks = new DeleteLinks();
    const searchLinks = new SearchLinks();

};

const linksIndexInstance = new LinksIndex();