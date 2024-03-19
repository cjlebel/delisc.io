import { LinkCards } from '@components/link-cards/index.ts';
import { ReactPager } from '@components/pager/index.ts';

export default function LinksIndex() {

    const linksIndex = document.getElementById('links-page');

    if (linksIndex) {
        console.log('Links Index loaded!!!!!');

        LinkCards();

        if (document.getElementById('pager-container') !== null)
            ReactPager('pager-container');
    }

};

document.addEventListener('DOMContentLoaded', LinksIndex);