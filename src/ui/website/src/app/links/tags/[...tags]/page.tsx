import React, { Suspense } from 'react';

import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { TagResult } from '@/types/tags';
import { LinkCards } from '@/components/elements/links';
import TagsCard from '@/components/elements/tags';

type LinksTagsPageProps = {
   tags: string;
};

const getTaggedLinks = async (tags: string, pageNo: number, size: number) => {
   var data = await fetch(`${API_URL}/links/${tags}/${pageNo}/${size}`);

   if (data.ok) {
      return await data.json();
   }
};

const getRelatedTags = async (tags: string, size: number) => {
   var data = await fetch(`${API_URL}/links/tags/${tags}/${size}`, { next: { revalidate: 10 } });

   if (data.ok) {
      return await data.json();
   }
};

export default async function LinksTagsPage({ params }: { params: LinksTagsPageProps }) {
   const tagsStr = params.tags ? decodeURIComponent(params.tags).replace('+', ' ') : '';

   const linksData: ResultsPage<LinkResult> = await getTaggedLinks(tagsStr, 1, 50);
   const tagsData: TagResult[] = await getRelatedTags(params.tags, 50);

   if (!linksData) {
      return `Error: 😢`;
   }
   return (
      <>
         <Suspense fallback={<>Loading...</>}>
            <section className={styles.content}>
               <LinkCards items={linksData.results} />

               <div>
                  Page {linksData.pageNumber} of {linksData.totalPages} ({linksData.totalResults}{' '}
                  Results)
               </div>
            </section>
         </Suspense>
         <Suspense fallback={<>Loading...</>}>
            <aside className={styles.sidebar}>
               <TagsCard preexisting={tagsStr} title='Related Tags' tags={tagsData} />
            </aside>
         </Suspense>
      </>
   );
}
