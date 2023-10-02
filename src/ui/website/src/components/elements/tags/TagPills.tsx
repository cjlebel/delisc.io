import React from 'react';

import styles from './Tags.module.scss';

import { TagPill } from '@/components/atoms/tags';
import { TagResult } from '@/types/tags';

type TagPillsProps = {
   tags: TagResult[];
};

export default function TagPills({ tags }: TagPillsProps) {
   const options = [
      'bg-deliscio',
      'bg-primary',
      'bg-secondary',
      'bg-success',
      'bg-warning text-dark',
      'bg-info text-dark',
      'bg-light text-dark',
      'bg-deliscio text-dark',
      'bg-primary text-dark',
      'bg-secondary text-dark',
      'bg-success text-dark',
   ];

   const results = tags
      ? tags.map((tag: TagResult, index: number) => {
           const option =
              index <= options.length ? options[index] : options[index % options.length];

           return <TagPill key={tag.name} tag={tag} className={option} />;
        })
      : [];

   return (
      <>
         <span className={styles['tag-pills']}>{results}</span>
      </>
   );
}
