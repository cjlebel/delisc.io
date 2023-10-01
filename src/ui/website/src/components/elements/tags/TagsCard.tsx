import React from 'react';
import Link from 'next/link';

import { TagResult } from '@/types/tags';

/**
 * @param {string} title - The title of the card
 * @param {Tag[]} tags - The tags to display
 * @param {string} preexisting - A comma-separated list of tags that already existed prior to the passed in tags.
 *                               This is used to filter the fetched collection of tags, so that these ones won't show again
 */
type PopularTagsCardProps = {
   preexisting?: string;
   title: string;
   tags: TagResult[];
};

function TagsCard({ title, tags, preexisting }: PopularTagsCardProps) {
   if (!tags) return <></>;

   // If there's an existing list of tags
   const newTags = preexisting
      ? tags.filter((t) => !preexisting.split(',').includes(t.name))
      : tags;

   const maxWeight = Math.max(...tags.map((tag) => tag.weight));

   const tagItems = newTags.map((tag) => {
      const tagSize = tag.weight / maxWeight >= 0.3 ? (tag.weight / maxWeight) * 1 + 0.5 : 0.8;

      const sanitizedTag = preexisting
         ? `${preexisting.replace(/ /g, '+')},${tag.name.replace(/ /g, '+')}`
         : `${tag.name.replace(/ /g, '+')}`;

      const href = `/links/tags/${sanitizedTag}`;

      return (
         //  <li key={tag.name} className='d-inline'>
         <li key={tag.name}>
            <Link
               href={href}
               data-count={tag.count}
               data-weight={tag.weight}
               data-percent={tag.weight / maxWeight}
               style={{ fontSize: `${tagSize}rem` }}>
               {tag.name}
            </Link>
            &nbsp;
         </li>
      );
   });

   return (
      <div className={`card`} style={{ width: '100%' }}>
         <div className='card-header'>{title}</div>
         <div className='card-body'>
            <ul className='list-unstyled'>{tagItems}</ul>
         </div>
      </div>
   );
}

export default TagsCard;
