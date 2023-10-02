import React from 'react';
import Link from 'next/link';

import styles from './tags.module.scss';

import { TagResult } from '@/types/tags';

type TagPillProps = {
   tag: TagResult;
   className?: string;
};

export default function TagPill({ tag, className }: TagPillProps) {
   const css = className ? `badge rounded-pill ${className}` : `badge rounded-pill bg-deliscio`;

   const sanitizedTag = `${tag.name.replace(/ /g, '+')}`;

   const href = `/links/tags/${sanitizedTag}`;

   return (
      <span key={tag.name} className={css}>
         <Link href={href}>{tag.name}</Link>
         {/* {tag.name} */}
      </span>
   );
}
