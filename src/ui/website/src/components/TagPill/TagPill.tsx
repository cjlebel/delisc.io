import React from 'react';
import Link from 'next/link';

import styles from './tags.module.scss';

export default function TagPill({ name, className, href, count, totalCount }: TagPillProps) {
   const css = `${styles.tag} ${className ? styles[className] : ''}`;

   const sanitizedTag = `${name.replace(/ /g, '+')}`;

   href = `/tags/${sanitizedTag}`;

   const tagSize =
      count === totalCount
         ? 1
         : totalCount > 0 && count / totalCount >= 0.0001
         ? 1 + (count / totalCount) * 5
         : 0.8;

   return (
      <span key={name} className={css}>
         <Link
            href={href}
            style={{
               fontSize: `${tagSize}rem`,
            }}
            title={`${name} Tag`}>
            {name}
         </Link>
      </span>
   );
}

type TagPillProps = {
   name: string;
   className?: string;
   href: string;
   count: number;
   totalCount: number;
};
