import React from 'react';
import Image from 'next/image';
import Link from 'next/link';

import styles from './LinkCard.module.scss';
import { LinkItemResult } from '@/types/links';
import { TagResult } from '@/types/tags';

export default function LinkCard(item: LinkItemResult) {
   const imgUrl = item.imageUrl
      ? item.imageUrl
      : 'https://dummyimage.com/300x200/ff0000?text=Placeholder';

   const title = item.title.length > 50 ? item.title.slice(0, 47) + '...' : item.title;
   const description =
      item.description.length > 100 ? item.description.slice(0, 97) + '...' : item.description;

   const tagItems = item.tags
      ? item.tags.map((tag: TagResult) => {
           return (
              <span key={tag.name} className={styles.tag}>
                 {/* <Link href={`/links/tags/${tag.name.replace(/ /g, '+')}`}>{tag.name}</Link> */}
                 {tag.name}
              </span>
           );
        })
      : [];

   const tagFooter =
      tagItems?.length > 0 ? (
         <div
            className={`card-footer ${styles.tags}`}
            style={{ width: '100%', minHeight: '42.5px' }}>
            {tagItems}
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
