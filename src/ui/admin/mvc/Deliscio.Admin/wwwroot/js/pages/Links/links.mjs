/**
 * Responsible for managing the Links > Index page
 */
import DeleteLinks from "./components/links-delete.mjs";

export default function LinksIndex() {
    const deleteLinks = new DeleteLinks();
};

const linksIndexInstance = new LinksIndex();