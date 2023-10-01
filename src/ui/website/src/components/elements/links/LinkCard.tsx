import React from 'react';
import Image from 'next/image';
import Link from 'next/link';

import styles from './LinkCard.module.scss';
import { LinkItemResult } from '@/types/links';

export default function LinkCard(item: LinkItemResult) {
   const imgUrl = item.imageUrl
      ? item.imageUrl
      : 'https://dummyimage.com/300x200/ff0000?text=Placeholder';

   const title =
      item.title.length > 50 ? item.title.slice(0, 50) + '...' : item.title;
   const description =
      item.description.length > 100
         ? item.description.slice(0, 100) + '...'
         : item.description;
   // const tags = props.tags?.map((tag) => {
   //    return (
   //       <div key={tag} className={styles.tag}>
   //          {tag["name"]}
   //       </div>
   //    );
   // });

   return (
      <Link href={`/links/link/${item.id}`} className={styles['link-item']}>
         <div className={`card`}>
            <Image
               src={imgUrl}
               className='card-img-top'
               alt={title}
               width={0}
               height={0}
               sizes='100vw'
               style={{ width: '100%', height: 'auto' }}
            />
            <div className='card-body'>
               <h5 className={`card-title ${styles.title}`}>{title}</h5>
               <p>
                  via : <span>{item.domain}</span>
               </p>
               <p className={`card-text ${styles.description}`}>
                  {description}
               </p>
            </div>
            <div className={`card-footer ${styles.tags}`}>
               <span className={styles.tag}>Tag1</span>
               <span className={styles.tag}>Tag2</span>
               <span className={styles.tag}>Tag3</span>
            </div>
         </div>
      </Link>
      //   <div className={styles['link-item']}>
      //      <Link href={`/links/link/${props.id}`}>
      //         <div>
      //            <Image
      //               src={imgUrl}
      //               alt={title}
      //               width={0}
      //               height={0}
      //               sizes='100vw'
      //               style={{ width: '100%', height: 'auto' }}
      //            />
      //            <div className={styles.title}>{title}</div>
      //            <p className={styles.description}>{description}</p>
      //         </div>
      //         <div className={styles.tags}>
      //            <span className={styles.tag}>Tag1</span>
      //            <span className={styles.tag}>Tag2</span>
      //            <span className={styles.tag}>Tag3</span>
      //         </div>
      //      </Link>
      //   </div>
   );
}
