import React, { Suspense } from 'react';
import styles from './page.module.scss';

import { apiGetLinks } from '@/apis';

import { LinkCards } from '@/components/elements/links';
import { PopularRelatedTags } from '@/components/molecules/PopularRelatedTags';
import { Pager } from '@/components/elements/navigation';

export default async function LinksTagsPage({
   params,
   searchParams,
}: {
   params: LinksTagsPageProps;
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   const page = searchParams?.page ? parseInt(searchParams.page as string) : 1;

   const links = await apiGetLinks({
      page: page,
      tags: params.tags,
   });

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
               <PopularRelatedTags />
            </Suspense>
         </aside>
      </>
   );
}

type LinksTagsPageProps = {
   tags: string[];
};
