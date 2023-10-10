'use client';
import Link from 'next/link';
import React from 'react';
import { usePathname } from 'next/navigation';
import { useSearchParams } from 'next/navigation';
import styles from './Pager.module.scss';

type PagerProps = {
   currentPage: number;
   totalPages: number;
   totalResults?: number;
};

/**
 * A simple pager component to traverse back and forth through the pages (when applicable)
 * @param props
 * @returns
 */
export default function Pager(props: PagerProps) {
   // Used to get the path of the url (excluding query strings)
   const pathName = usePathname();
   // Used to get the query strings
   const searchParams = useSearchParams();

   const tags = searchParams?.get('tags')?.replaceAll(' ', '+') ?? '';

   let baseHref = tags ? `${pathName}?tags=${tags}` : '${pathName}?';

   let query = new URLSearchParams();

   if (tags?.trim() !== '') {
      query.append('tags', tags);
   }

   const firstPage =
      props.currentPage > 1 ? (
         <li className='first' style={{}}>
            <Link
               className='page-link'
               href={query.size > 0 ? `${pathName}?${query.toString}` : `/`}
               tabIndex={-1}
               aria-disabled='false'
               style={{ borderRadius: '1rem' }}>
               First
            </Link>
         </li>
      ) : (
         <li className='first disabled' style={{}}>
            <Link
               className='page-link'
               href={`/`}
               tabIndex={-1}
               style={{ borderRadius: '1rem' }}
               aria-disabled='true'>
               First
            </Link>
         </li>
      );

   const prevPage =
      props.currentPage > 1 ? (
         <li className='previous' style={{}}>
            <Link
               className='page-link'
               href={
                  query.size > 0
                     ? `${pathName}?${query.toString}&page=${props.currentPage - 1}`
                     : `${pathName}?page=${props.currentPage - 1}`
               }
               tabIndex={-1}
               aria-disabled='false'
               style={{ borderRadius: '1rem' }}>
               Previous
            </Link>
         </li>
      ) : (
         <li className='previous disabled' style={{}}>
            <Link
               className='page-link'
               href={`/`}
               tabIndex={-1}
               style={{ borderRadius: '1rem' }}
               aria-disabled='true'>
               Previous
            </Link>
         </li>
      );

   const nextPage =
      props.currentPage < props.totalPages ? (
         <li className='next' style={{}}>
            <Link
               className='page-link'
               href={`${pathName}?page=${props.currentPage + 1}`}
               style={{ borderRadius: '1rem' }}
               aria-disabled='false'>
               Next
            </Link>
         </li>
      ) : (
         <li className='next disabled' style={{}}>
            <Link
               className='page-link'
               href={`/`}
               aria-disabled='true'
               style={{ borderRadius: '1rem' }}>
               Next
            </Link>
         </li>
      );

   const lastPage =
      props.currentPage < props.totalPages ? (
         <li className='last' style={{}}>
            <Link
               className='page-link'
               href={`${pathName}?page=${props.totalPages}`}
               style={{ borderRadius: '1rem' }}
               aria-disabled='false'>
               Last
            </Link>
         </li>
      ) : (
         <li className='last disabled' style={{}}>
            <Link
               className='page-link'
               href={`/`}
               aria-disabled='true'
               style={{ borderRadius: '1rem' }}>
               Last
            </Link>
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
