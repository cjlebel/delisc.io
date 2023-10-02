import React, { useState } from 'react';
import Image from 'next/image';
import Link from 'next/link';

import styles from './LinkCard.module.scss';
import { LinkItemResult } from '@/types/links';
import TagPills from '../tags/TagPills';

export default function LinkCard(item: LinkItemResult) {
   //const [src, setSrc] = useState('https://i.imgur.com/gf3TZMr.jpeg');

   const imgUrl = item.imageUrl
      ? item.imageUrl
      : item.url.indexOf('duckduckgo') >= 0
      ? '/duckduckgo-logo.jpg'
      : item.url.indexOf('google') >= 0
      ? '/google-logo.jpg'
      : item.url.indexOf('reddit') >= 0
      ? '/reddit-logo.jpg'
      : 'https://dummyimage.com/300x200/1C2128?text=:(';

   const title = item.title.length > 50 ? item.title.slice(0, 47) + '...' : item.title;
   const description =
      item.description.length > 100 ? item.description.slice(0, 97) + '...' : item.description;

   var tagPills = item.tags ? <TagPills tags={item.tags} /> : null;

   const tagFooter = tagPills ? (
      <div
         className={`card-footer ${styles['footer-tags']}`}
         style={{ width: '100%', minHeight: '42.5px' }}>
         {tagPills}
      </div>
   ) : (
      <div></div>
   );

   return (
      <div className={`${styles['link-item']} card`}>
         <Link href={`/links/link/${item.id}`}>
            <Image
               src={imgUrl}
               className='card-img-top'
               alt={title}
               width={0}
               height={0}
               sizes='100vw'
               style={{ width: '100%', height: 'auto' }}
               //    onError={() => setSrc('https://dummyimage.com/300x200/ff0000?text=Placeholder')}
            />
            <div className={`card-body ${styles['body']}`}>
               <h5 className={`card-title ${styles.title}`}>{title}</h5>
               <p>
                  via : <span>{item.domain}</span>
               </p>
               <p className={`card-text ${styles.description}`}>{description}</p>
            </div>
         </Link>
         {tagFooter}
      </div>
   );
}
