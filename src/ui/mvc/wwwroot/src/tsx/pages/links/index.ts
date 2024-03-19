import LinksIndex from './links-index';
import LinksDetails from './links-details';


export default function Links() {

    if (document.getElementById('links-page')) {

        LinksIndex();

    };

    if (document.getElementById('links-details-page')) {

        LinksIndex();

    };

    //if (window.location.pathname === '/links/details') {
    //    LinksDetails();
    //};

};

document.addEventListener('DOMContentLoaded', Links);
