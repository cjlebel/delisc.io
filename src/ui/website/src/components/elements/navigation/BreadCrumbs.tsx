'use client';
import React from 'react';
import { usePathname } from 'next/navigation';

import styles from './BreadCrumbs.module.scss';

import Link from 'next/link';

type BreadCrumbsProps = {};

export default function BreadCrumbs({}: BreadCrumbsProps) {
   const pathName = usePathname(); //.toLowerCase().replace('/link/', '/');

   let parts = pathName.split('/');

   if (parts.length == 1) {
      return <div>Home</div>;
   }

   const isLinkPage = parts[1] === 'link';

   let path = '';
   let foreColor = 'text-primary';

   // Note: / aka Home, isn't part of the parts array
   const breadcrumbs = parts.map((part, index) => {
      path += index == 1 ? `${part}` : `/${part}`;

      const part2 = decodeURIComponent(part).replace('+', ' ');
      foreColor = index == parts.length - 1 ? 'deliscio' : 'text-primary';
      if (index < parts.length - 1) {
         return (
            <React.Fragment key={index}>
               <Link
                  key={index}
                  href={path.replace(' ', '+').toLowerCase()}
                  className={foreColor}
                  title={part2}>
                  {part2}
               </Link>
               {index < parts.length && <span> / </span>}
            </React.Fragment>
         );
      }
      return (
         <React.Fragment key={index}>
            <span className={foreColor}>{part2}</span>
         </React.Fragment>
      );
   });

   return (
      <div className={`${styles.breadcrumbs} container-fluid`}>
         <Link href='/' className={'text-primary'} title='Home'>
            Home
         </Link>{' '}
         {breadcrumbs}
      </div>
   );
}
