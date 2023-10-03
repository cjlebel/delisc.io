import React, { Suspense } from 'react';
import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { TagResult } from '@/types/tags';
import { LinkCards } from '@/components/elements/links';
import { PopularRecentTags } from '@/components/elements/tags';
import { Pager } from '@/components/elements/pager';

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

export default async function LinksTagsPage({
   params,
   searchParams,
}: {
   params: LinksTagsPageProps;
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   const pageNo = searchParams?.page ? parseInt(searchParams.page as string) : 1;
   const tagsStr = params.tags
      ? decodeURIComponent(params.tags.join(',')).replaceAll('+', ' ')
      : '';

   const linksData: ResultsPage<LinkResult> = await getTaggedLinks(tagsStr, pageNo, 50);
   const tagsData: TagResult[] = await getRelatedTags(tagsStr, 50);

   return (
      <>
         <section className={styles.content}>
            <Suspense fallback={<>Loading...</>}>
               {linksData?.results?.length >= 0 ? (
                  <LinkCards items={linksData.results} />
               ) : (
                  <div>No links found {tagsStr}</div>
               )}
            </Suspense>
            {linksData ? (
               <Pager
                  currentPage={linksData.pageNumber}
                  totalPages={linksData.totalPages}
                  totalResults={linksData.totalResults}
               />
            ) : null}
         </section>
         <aside className={`sidebar ${styles.sidebar}`}>
            <Suspense fallback={<>Loading...</>}>
               <PopularRecentTags baseApi={API_URL} count={50} />
            </Suspense>
         </aside>
      </>
   );
}

type LinksTagsPageProps = {
   tags: string[];
};
