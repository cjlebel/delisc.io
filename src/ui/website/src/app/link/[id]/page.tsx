import React, { Suspense } from 'react';
import Image from 'next/image';
import Link from 'next/link';

import styles from './page.module.scss';

import { ServerSideApis } from '@/apis/clientside/LinksApis';

import TagPills from '@/components/elements/tags/TagPills';
import { LinkItemResult } from '@/types/links';
import ContentWithRightSideBarTemplate from '@/components/templates/ContentWithRightSideBar';

type LinkPageProps = {
   id: string;
};

export default async function LinkPage({ params }: { params: LinkPageProps }) {
   const link: LinkItemResult = await ServerSideApis.LinksApis.getLink(params.id);

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

   const getMainContent = (): React.ReactNode => {
      return (
         <>
            <h1>{title}</h1>
            <Image
               src={imgUrl}
               className='card-img-top'
               alt={link.title}
               width={0}
               height={0}
               sizes='100vw'
               style={{ width: '100%', height: 'auto' }}
            />
            {/* <div className='position-absolute bottom-0 start-0 end-0 p-3'> */}
            <div className='d-flex justify-content-between pt-2 pb-2'>
               <div className=''>
                  <span className='badge bg-primary'>Dinner</span>&nbsp;
                  <span className='badge bg-primary'>Italian</span>
               </div>

               <div>
                  <a
                     href='original-source-url.com'
                     className='btn btn-outline-danger'
                     target='_blank'>
                     Original Source
                  </a>
               </div>

               <div className=''>
                  <button className='btn btn-outline-primary'>Like</button>&nbsp;
                  <button className='btn btn-outline-secondary'>Save</button>
               </div>
            </div>

            <p>{link.description}</p>

            <h3 className='mt-4'>Personal Notes</h3>
            <p>User&apos;s notes about the recipe go here.</p>

            <h3 className='mt-4'>Comments</h3>
            <div className='mb-4'>{/*<!--  Comments from users go here --> */}</div>
         </>
      );
   };

   const getRightSideContent = (): React.ReactNode => {
      return (
         <>
            <div className='flex-shrink-0' style={{ width: '300px' }}>
               <div>
                  <h3>Notes</h3>
                  <p>Notes ....</p>
               </div>
               <div>
                  <h3>Related Links</h3>
                  <ul className='list-group'>
                     <li className='list-group-item'>Related Link 1</li>
                     <li className='list-group-item'>Related Link 2</li>
                  </ul>
               </div>
            </div>
         </>
      );
   };

   return (
      <ContentWithRightSideBarTemplate main={getMainContent()} rightSide={getRightSideContent()} />
   );
}
