import React from 'react';

import { LinkResult } from '@/types/links';
import LinkCard from './LinkCard';

type LinkCardsProps = {
   items: LinkResult[];
};

export default function LinkCards({ items }: LinkCardsProps) {
   return (
      <>
         <div className='links-list d-flex flex-wrap' style={{ gap: '10px' }}>
            {items.map((link: any, idx: number) => {
               const priority = idx < 6;
               return <LinkCard key={link.id} {...link} priority={priority} />;
            })}
         </div>
      </>
   );
}
