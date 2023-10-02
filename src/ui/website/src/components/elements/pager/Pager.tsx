'use client';
import { usePathname } from 'next/navigation';

import Link from 'next/link';
import React from 'react';

type PagerProps = {
   currentPage: number;
   totalPages: number;
   totalResults?: number;
};

export default function Pager(props: PagerProps) {
   const pathName = usePathname();

   let parts = pathName.split('/');

   const prevPage =
      props.currentPage > 1 ? (
         <li className='previous'>
            <Link
               className='page-link'
               href={`${pathName}?page=${props.currentPage - 1}`}
               tabIndex={-1}
               aria-disabled='false'
               style={{ borderRadius: '1rem' }}>
               Previous
            </Link>
         </li>
      ) : (
         <li className='previous disabled'>
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
         <li className='next'>
            <Link
               className='page-link'
               href={`${pathName}?page=${props.currentPage + 1}`}
               style={{ borderRadius: '1rem' }}
               aria-disabled='false'>
               Next
            </Link>
         </li>
      ) : (
         <li className='next disabled'>
            <Link
               className='page-link'
               href={`/`}
               aria-disabled='true'
               style={{ borderRadius: '1rem' }}>
               Next
            </Link>
         </li>
      );

   const totalResults = props.totalResults ? (
      <li style={{ padding: '5px 10px' }}>Total Results: {props.totalResults}</li>
   ) : (
      <li style={{ padding: '5px 10px' }}></li>
   );

   return (
      <nav aria-label='Page navigation' className='d-flex flex-row' style={{ width: '100%' }}>
         <ul className='pagination justify-content-center'>
            {prevPage}
            {totalResults}
            {nextPage}
         </ul>
      </nav>
   );
}
