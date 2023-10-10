import React from 'react';

import styles from './Tags.module.scss';

import { TagPill } from '@/components/TagPill';
import { TagResult } from '@/types/tags';

type TagPillsProps = {
   tags: TagResult[];
};

export default function TagPills({ tags }: TagPillsProps) {
   let totalCount = 0;

   tags.forEach((tag) => {
      totalCount += tag.count;
   });

   const results = tags
      ? tags.map((tag: TagResult, idx: number) => {
           const tagId = `tag-${(idx % 10) + 1}`;
           const href = `/tags/${tag.name}`;

           return (
              <TagPill
                 key={tag.name}
                 href={href}
                 name={tag.name}
                 className={tagId}
                 count={tag.count}
                 totalCount={tag.count}
              />
           );
        })
      : [];

   return (
      <>
         <span className={styles['tag-pills']}>{results}</span>
      </>
   );
}
