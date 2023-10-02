import React from 'react';
import Link from 'next/link';

import styles from './Tags.module.scss';

import { TagResult } from '@/types/tags';

/**
 * @param {string} title - The title of the card
 * @param {Tag[]} tags - The tags to display
 * @param {string} preexisting - A comma-separated list of tags that already existed prior to the passed in tags.
 *                               This is used to filter the fetched collection of tags, so that these ones won't show again
 */
type TagsCardProps = {
   preexisting?: string;
   title: string;
   tags: TagResult[];
};

const options = [
   'bg-deliscio',
   'bg-primary',
   'bg-secondary',
   'bg-success',
   'bg-warning text-dark',
   'bg-info text-dark',
   //    'bg-light text-dark',
   'bg-deliscio text-dark',
   'bg-primary text-dark',
   'bg-secondary text-dark',
   'bg-success text-dark',
];

function TagsCard({ title, tags, preexisting }: TagsCardProps) {
   if (!tags) return <></>;

   // If there's an existing list of tags
   const newTags = preexisting
      ? tags.filter((t) => !preexisting.split(',').includes(t.name))
      : tags;

   const maxWeight = Math.max(...tags.map((tag) => tag.weight));

   const tagItems = newTags.map((tag, index) => {
      const tagSize = tag.weight / maxWeight >= 0.0001 ? (tag.weight / maxWeight) * 1 + 1.5 : 0.8;
      const option = index <= options.length ? options[index] : options[index % options.length];

      // preexisting tags is a comma-separated list of tags. Need to replace , with /
      const sanitizedTag = preexisting
         ? // This mess is due to me having a tag with a / in it (which I will need to prevent)
           `${preexisting.replace(',', '/').replace(/ /g, '+')}/${tag.name
              .replace('/', '%2F')
              .replace(/ /g, '+')}`
         : `${tag.name.replace('/', '%2F').replace(/ /g, '+')}`;

      const href = `/links/tags/${sanitizedTag}`;

      return (
         <li
            key={tag.name}
            style={{ display: 'inline-block', marginRight: '10px', marginBottom: '10px' }}>
            <Link
               href={href}
               className={option}
               data-count={tag.count}
               data-weight={tag.weight}
               data-percent={tag.weight / maxWeight}
               style={{
                  fontSize: `${tagSize}rem`,
                  padding: '5px',
                  borderRadius: '0.5rem',
                  wordBreak: 'break-word',
               }}>
               {tag.name}
            </Link>
            &nbsp;
         </li>
      );
   });

   return (
      <div className={`card ${styles.card}`} style={{ width: '100%' }}>
         <div className={`card-header ${styles['card-header']}`}>{title}</div>
         <div className='card-body'>
            <ul className='list-unstyled'>{tagItems}</ul>
         </div>
      </div>
   );
}

export default TagsCard;
