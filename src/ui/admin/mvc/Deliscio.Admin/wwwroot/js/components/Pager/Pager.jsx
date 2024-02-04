import React from 'react';
import PropTypes from 'prop-types';
import styles from './Pager.module.scss';

Pager.propTypes = {
    currentPage: PropTypes.number.isRequired,
    totalPages: PropTypes.number.isRequired,
    totalResults: PropTypes.number,
};

export default function Pager(props) {
    const baseUrl = '/';

    const firstPage =
        props.currentPage > 1 ? (
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
        props.currentPage > 1 ? (
            <li className='previous' style={{}}>
                <a
                    className='page-link'
                    href={`/?page=${props.currentPage - 1}`}
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
        props.currentPage < props.totalPages ? (
            <li className='next' style={{}}>
                <a
                    className='page-link'
                    href={`/?page=${props.currentPage + 1}`}
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
        props.currentPage < props.totalPages ? (
            <li className='last' style={{}}>
                <a
                    className='page-link'
                    href={`/?page=${props.totalPages}`}
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

    const totalResults = props.totalResults ? (
        <li style={{ padding: '5px 10px' }}>Total Results: {props.totalResults}</li>
    ) : (
        <li style={{ padding: '5px 10px' }}></li>
    );

    return (
        <nav className={styles.pager} aria-label='Page navigation'>
            <ul className='pagination justify-content-center' style={{ width: '100%' }}>
                {props.totalPages > 1 ? (
                    <>
                        {firstPage} {prevPage} {totalResults} {nextPage} {lastPage}
                    </>
                ) : null}
            </ul>
        </nav>
    );
}
