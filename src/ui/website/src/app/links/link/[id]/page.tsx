import React, { Suspense } from 'react';
import Image from 'next/image';
import Link from 'next/link';

import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { LinkResult } from '@/types/links';
import TagPills from '@/components/elements/tags/TagPills';

type LinkPageProps = {
   id: string;
};

const getLink = async (id: string) => {
   var data = await fetch(`${API_URL}/link/${id}`, { next: { revalidate: 10 } });

   if (data.ok) {
      return await data.json();
   }
};

export default async function LinkPage({ params }: { params: LinkPageProps }) {
   var linkData: LinkResult = await getLink(params.id);

   if (!linkData) {
      return <div>Link not found</div>;
   }

   var title = linkData.title
      ? linkData.title.length > 100
         ? linkData.title.substring(0, 97) + '...'
         : linkData.title
      : '';

   const imgUrl = linkData.imageUrl
      ? linkData.imageUrl
      : 'https://dummyimage.com/600x400/ff0000?text=Placeholder';

   const getFooter = () => {
      const tags = linkData.tags ? <TagPills tags={linkData.tags} /> : null;
      return <div>{tags}</div>;
   };

   return (
      <Suspense fallback={<>Loading...</>}>
         <section className={styles.content}>
            <div className='content d-flex flex-wrap' style={{ gap: '10px', padding: '0 10px' }}>
               <Image
                  src={imgUrl}
                  className='card-img-top'
                  alt={linkData.title}
                  width={0}
                  height={0}
                  sizes='100vw'
                  style={{ width: '100%', height: 'auto' }}
               />
               <div className={`card`}>
                  <div className='card-body'>
                     <h1 className='card-title'>{title}</h1>
                     <p className='card-text'>{linkData.description}</p>
                     <p>
                        Link:{' '}
                        <Link href={linkData.url} style={{ color: 'red' }} target='_blank'>
                           {title}
                        </Link>
                     </p>
                  </div>
                  <div
                     className={`card-footer ${styles.tags}`}
                     style={{ width: '100%', minHeight: '42.5px' }}>
                     {getFooter()}
                  </div>
               </div>
            </div>
         </section>
         <aside className={`sidebar ${styles.sidebar}`}>some stuff</aside>
      </Suspense>
   );
}
