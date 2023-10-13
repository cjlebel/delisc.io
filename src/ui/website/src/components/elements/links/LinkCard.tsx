'use client';

import React, { useEffect, useState } from 'react';
import Image from 'next/image';
import Link from 'next/link';

import styles from './LinkCard.module.scss';

import { LinkItemResult } from '@/types/links';
import TagPills from '../tags/TagPills';

const brokenImg = '/no-image-found.png';

export default function LinkCard(item: LinkItemResult, priority: boolean = false) {
   const [src, setSrc] = useState('/blank-image.png');

   useEffect(() => {
      item.imageUrl = item.imageUrl.startsWith('//') ? `https:${item.imageUrl}` : item.imageUrl;

      const imgUrl =
         item.imageUrl && item.imageUrl.length > 0 && item.imageUrl.startsWith('http')
            ? item.imageUrl
            : item.url.indexOf('duckduckgo') >= 0
            ? '/duckduckgo-logo.jpg'
            : item.url.indexOf('google') >= 0
            ? '/google-logo.jpg'
            : item.url.indexOf('reddit') >= 0
            ? '/reddit-logo.png'
            : '/no-image-found.png';

      setSrc(imgUrl);
   }, [item]);

   const title = item.title.length > 50 ? item.title.slice(0, 47) + '...' : item.title;
   const description =
      item.description.length > 100 ? item.description.slice(0, 97) + '...' : item.description;

   var tagPills = item.tags && item.tags.length > 0 ? <TagPills tags={item.tags} /> : null;

   const tagFooter = tagPills ? (
      <div className={`${styles['footer']}`}>{tagPills}</div>
   ) : (
      <div></div>
   );
   console.log(src);

   const onImgError = (e: any) => {
      console.log(e);
      setSrc(brokenImg);
   };

   return (
      <div className={`${styles['link-tile']}`}>
         <Link href={`/link/${item.id}`}>
            <Image
               src={src}
               className='card-img-top'
               alt={title}
               width={0}
               height={0}
               sizes='100vw'
               style={{ width: '100%', height: 'auto' }}
               //    priority={priority}
               onError={(e) => onImgError(e)}
            />
            <div className={`${styles['body']}`}>
               <h5 className={`${styles.title}`}>{title}</h5>
               <p>
                  via : <span>{item.domain}</span>
               </p>
               <p className={`${styles.description}`}>{description}</p>
            </div>
         </Link>
         {tagFooter}
      </div>
   );
}
