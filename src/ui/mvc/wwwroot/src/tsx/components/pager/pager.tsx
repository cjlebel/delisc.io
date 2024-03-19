import React from 'react';

//type Props = {
//    currentPage: number;
//    totalPages: number;
//    totalResults?: number;
//};

type Props = {
    attachTo: string;
}

export default function Pager(props: Readonly<Props>) {
    const element = document.getElementById(props.attachTo);

    if (!element)
        return;

    const baseUrl = element.getAttribute('data-base-url')

    const currentPage = Number(element.getAttribute('data-pageNo'));
    const size = Number(element.getAttribute('data-size'));
    const totalPages = Number(element.getAttribute('data-pages'));
    const totalResults = Number(element.getAttribute('data-total-results'));



    const firstPage =
        currentPage > 1 ? (
            <li className='first' style={{}}>
                <a
                    className='page-link'
                    href={`/`}
                    tabIndex={-1}
                    aria-disabled='false'
                    style={{ borderRadius: '1rem' }}>
                    First
                </a>
            </li>
        ) : (
            <li className='first disabled' style={{}}>
                <a
                    className='page-link'
                    href={`/`}
                    tabIndex={-1}
                    style={{ borderRadius: '1rem' }}
                    aria-disabled='true'>
                    First
                </a>
            </li>
        );

    const prevPage =
        currentPage > 1 ? (
            <li className='previous' style={{}}>
                <a
                    className='page-link'
                    href={`/?page=${currentPage - 1}`}
                    tabIndex={-1}
                    aria-disabled='false'
                    style={{ borderRadius: '1rem' }}>
                    Previous
                </a>
            </li>
        ) : (
            <li className='previous disabled' style={{}}>
                <a
                    className='page-link'
                    href={`/`}
                    tabIndex={-1}
                    style={{ borderRadius: '1rem' }}
                    aria-disabled='true'>
                    Previous
                </a>
            </li>
        );

    const nextPage =
        currentPage < totalPages ? (
            <li className='next' style={{}}>
                <a
                    className='page-link'
                    href={`/?page=${currentPage + 1}`}
                    style={{ borderRadius: '1rem' }}
                    aria-disabled='false'>
                    Next
                </a>
            </li>
        ) : (
            <li className='next disabled' style={{}}>
                <a
                    className='page-link'
                    href={`/`}
                    aria-disabled='true'
                    style={{ borderRadius: '1rem' }}>
                    Next
                </a>
            </li>
        );

    const lastPage =
        currentPage < totalPages ? (
            <li className='last' style={{}}>
                <a
                    className='page-link'
                    href={`/?page=${totalPages}`}
                    style={{ borderRadius: '1rem' }}
                    aria-disabled='false'>
                    Last
                </a>
            </li>
        ) : (
            <li className='last disabled' style={{}}>
                <a
                    className='page-link'
                    href={`/`}
                    aria-disabled='true'
                    style={{ borderRadius: '1rem' }}>
                    Last
                </a>
            </li>
        );

    totalResults ? (
        <li style={{ padding: '5px 10px' }}>Total Results: {totalResults}</li>
    ) : (
        <li style={{ padding: '5px 10px' }}></li>
    );

    return (
        <nav className='pager' aria-label='Page navigation'>
            <ul className='pagination justify-content-center' style={{ width: '100%' }}>
                {totalPages > 1 ? (
                    <>
                        {firstPage} {prevPage} {totalResults} {nextPage} {lastPage}
                    </>
                ) : null}
            </ul>
        </nav>
    );
}
