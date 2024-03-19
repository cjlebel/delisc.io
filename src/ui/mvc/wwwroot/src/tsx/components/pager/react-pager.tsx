import React from 'react';
import { createRoot } from 'react-dom/client';

import Pager from './pager';

export default function ReactPager(attachTo: string) {

    if (!attachTo)
        return;

    const node = document.getElementById(attachTo);

    if (node) {
        const root = createRoot(node);

        root.render(
            <React.StrictMode>
                <Pager attachTo={'pager-container'} />
            </React.StrictMode>
        );
    }
}