import React from 'react';
import Image from 'next/image';

import { ServerSideClientApis } from '@/apis/serverside/LinksApis';

import TagPills from '@/components/elements/tags/TagPills';
import { LinkItemResult } from '@/types/links';
import ContentWithRightSideBarTemplate from '@/components/templates/ContentWithRightSideBar';
import { GoToLinkButton, LikeLinkButton, SaveLinkButton } from '@/components/Buttons';

// , Consolas, 'Courier New', monospace
type LinkPageProps = {
   id: string;
};

export default async function LinkPage({ params }: { params: LinkPageProps }) {
   const link: LinkItemResult = await ServerSideClientApis.LinksApis.getLink(params.id);

   if (!link) {
      return <div>Link not found</div>;
   }

   var title = link.title
      ? link.title.length > 150
         ? link.title.substring(0, 147) + '...'
         : link.title
      : '';

   //TODO: Move to a utils class
   const imgUrl = link.imageUrl
      ? link.imageUrl
      : link.url.indexOf('duckduckgo') >= 0
      ? '/duckduckgo-logo.jpg'
      : link.url.indexOf('google') >= 0
      ? '/google-logo.jpg'
      : link.url.indexOf('reddit') >= 0
      ? '/reddit-logo.png'
      : '/no-image-found.png';

   const getLinkTags = () => {
      const tags = link.tags ? (
         <div className='flex-grow-1'>
            <TagPills tags={link.tags} />
         </div>
      ) : null;
      return <div>{tags}</div>;
   };

   const getMainContent = (): React.ReactNode => {
      return (
         <>
            <h1 style={{ fontSize: '2rem' }}>{title}</h1>
            <div className='d-flex flex-row'>
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
               <div className='px-4'>
                  <p>{link.description}</p>
                  {getLinkTags()}
                  {/* <div className='position-absolute bottom-0 start-0 end-0 p-3'> */}
                  <div
                     className='d-flex justify-content-between pt-2 pb-2'
                     style={{ position: 'relative', bottom: '0px' }}>
                     <div className=''>
                        <LikeLinkButton />
                     </div>
                     <div>
                        <SaveLinkButton />
                     </div>
                     <div>
                        <GoToLinkButton />
                     </div>
                  </div>
               </div>

               {/* <h3 className='mt-4'>Personal Notes</h3>
            <p></p> */}
            </div>
            <div>
               <h3 className='mt-4'>Comments</h3>
               <div className='mb-4'>{/*<!--  Comments from users go here --> */}</div>
            </div>
         </>
      );
   };

   const getRightSideContent = (): React.ReactNode => {
      return (
         <>
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
         </>
      );
   };

   return (
      <ContentWithRightSideBarTemplate main={getMainContent()} rightSide={getRightSideContent()} />
   );
}
