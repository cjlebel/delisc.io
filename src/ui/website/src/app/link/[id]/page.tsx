import React, { Suspense } from 'react';
import Image from 'next/image';
import Link from 'next/link';

import styles from './page.module.scss';

import { ClientSideApis } from '@/apis/LinksApis';

import TagPills from '@/components/elements/tags/TagPills';
import RelatedLinksPanel from '@/components/RelatedLinks/RelatedLinksPanel';
import { LinkItemResult, LinkResult } from '@/types/links';
import { ResultsPage } from '@/types/ResultsPage';

type LinkPageProps = {
   id: string;
};

export default async function LinkPage({ params }: { params: LinkPageProps }) {
   const link: LinkItemResult = await ClientSideApis.LinksApis.getLink(params.id);

   if (!link) {
      return <div>Link not found</div>;
   }

   const relatedTags: string[] = link?.tags?.map((tag) => {
      return tag.name;
   }) as string[];

   var title = link.title
      ? link.title.length > 150
         ? link.title.substring(0, 147) + '...'
         : link.title
      : '';

   const imgUrl = link.imageUrl
      ? link.imageUrl
      : link.url.indexOf('duckduckgo') >= 0
      ? '/duckduckgo-logo.jpg'
      : link.url.indexOf('google') >= 0
      ? '/google-logo.jpg'
      : link.url.indexOf('reddit') >= 0
      ? '/reddit-logo.png'
      : '/no-image-found.png';

   const getFooter = () => {
      const tags = link.tags ? <TagPills tags={link.tags} /> : null;
      return <div>{tags}</div>;
   };

   return (
      <Suspense fallback={<>Loading...</>}>
         <section className={styles.content}>
            <h1>{title}</h1>
            <div className='content d-flex flex-row'>
               <div>
                  <Image
                     src={imgUrl}
                     className='card-img-top'
                     alt={link.title}
                     width={0}
                     height={0}
                     sizes='100vw'
                     style={{ width: '100%', height: 'auto' }}
                  />
               </div>
               <div style={{ maxWidth: '33%', padding: '0 0.5em' }}>
                  <p className='description'>{link.description}</p>
               </div>
            </div>
            <div className={`footer ${styles.tags}`} style={{ width: '100%', minHeight: '2.5em' }}>
               <p>
                  Link:{' '}
                  <Link href={link.url} style={{ color: 'red' }} target='_blank'>
                     {title}
                  </Link>
               </p>
               {getFooter()}
            </div>
         </section>
         <aside className={`sidebar ${styles.sidebar}`}>
            <RelatedLinksPanel tags={relatedTags} />
         </aside>
      </Suspense>
   );
}
