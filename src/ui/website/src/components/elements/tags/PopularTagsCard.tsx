import React from 'react';
import Link from 'next/link';

import { Tag } from '@/types/tags';

type PopularTagsCardProps = {
   Tags: Tag[];
};

function PopularTagsCard({ Tags }: PopularTagsCardProps) {
   if (!Tags) return <></>;

   const maxWeight = Math.max(...Tags.map((tag) => tag.weight));

   const tagItems = Tags.map((tag) => {
      const tagSize =
         tag.weight / maxWeight >= 0.3
            ? (tag.weight / maxWeight) * 1 + 0.5
            : 0.8;

      const href = `/links/tags/${tag.name.replace(/ /g, '+')}`;

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
         <div className='card-header'>Popular Tags</div>
         <div className='card-body'>
            <ul className='list-unstyled'>{tagItems}</ul>
         </div>
      </div>
   );
}

export default PopularTagsCard;
