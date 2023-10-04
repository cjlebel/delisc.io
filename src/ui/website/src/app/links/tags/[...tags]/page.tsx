import React, { Suspense } from 'react';
import styles from './page.module.scss';

import { apiGetLinks } from '@/apis';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { LinkCards } from '@/components/elements/links';
import { PopularRecentTags } from '@/components/elements/tags';
import { Pager } from '@/components/elements/pager';

const getLinks = async (search?: string, tags?: string[], page?: number, count?: number) => {
   const data = await apiGetLinks({ page: page, count: count, tags: tags });

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
   const page = searchParams?.page ? parseInt(searchParams.page as string) : 1;

   //const linksData: ResultsPage<LinkResult> = await getTaggedLinks(tagsStr, page, 50);
   const links: ResultsPage<LinkResult> = await getLinks('', params.tags, page, 27);

   return (
      <>
         <section className={styles.content}>
            <Suspense fallback={<>Loading...</>}>
               {links?.results?.length >= 0 ? (
                  <LinkCards items={links.results} />
               ) : (
                  <div>No links found.</div>
               )}
            </Suspense>
            {links ? (
               <Pager
                  currentPage={links.pageNumber}
                  totalPages={links.totalPages}
                  totalResults={links.totalResults}
               />
            ) : null}
         </section>
         <aside className={`sidebar ${styles.sidebar}`}>
            <Suspense fallback={<>Loading...</>}>
               <PopularRecentTags count={23} />
            </Suspense>
         </aside>
      </>
   );
}

type LinksTagsPageProps = {
   tags: string[];
};
