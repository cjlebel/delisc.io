import { LinkCards } from '../../components/link-cards';
import { ReactPager } from '../../components/pager';

export default function Home() {
    const home = document.getElementById('home-page');

    if (home) {

        console.log('Home page loaded!!!!!');

        LinkCards();

        if (document.getElementById('pager-container') !== null)
            ReactPager('pager-container');
    };
};

// Should see if id exists before calling the function?
document.addEventListener('DOMContentLoaded', Home);