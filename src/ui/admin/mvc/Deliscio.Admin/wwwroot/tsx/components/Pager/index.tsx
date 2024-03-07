import React from 'react';
import ReactDOM from 'react-dom';

import PagerComponent from './PagerComponent';

export default function Pager() {
    const container = document.getElementById('pager-container');

    if (container) {

        const currentPage: number = parseInt(container.dataset.currentPage ?? "0");
        const totalPages: number = parseInt(container.dataset.totalPages ?? "0");
        const totalResults: number = parseInt(container.dataset.totalResults ?? "0");

        ReactDOM.render(<PagerComponent currentPage={currentPage} totalPages={totalPages} totalResults={totalResults} />, container);
    };
}