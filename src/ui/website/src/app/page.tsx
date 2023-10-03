import { Suspense } from 'react';

import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { TagResult } from '@/types/tags';

import LinkCards from '@/components/elements/links/LinkCards';
import { PopularRecentTags } from '@/components/elements/tags';
import { Pager } from '@/components/elements/pager';

const getLinks = async (pageNo: number, size: number) => {
   var data = await fetch(`${API_URL}/links/${pageNo}/${size}`);

   if (data.ok) {
      return await data.json();
   }
};

export default async function Home({
   searchParams,
}: {
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   const pageNo = searchParams?.page ? parseInt(searchParams.page as string) : 1;

   const linksData: ResultsPage<LinkResult> = await getLinks(pageNo, 27);

   return (
      <>
         <section className={styles.content}>
            <Suspense fallback={<>Loading...</>}>
               <LinkCards items={linksData.results} />
            </Suspense>
            <Pager
               currentPage={linksData.pageNumber}
               totalPages={linksData.totalPages}
               totalResults={linksData.totalResults}
            />
         </section>
         <aside className={`sidebar ${styles.sidebar}`}>
            <Suspense fallback={<>Loading...</>}>
               <PopularRecentTags baseApi={API_URL} count={200} />
            </Suspense>
         </aside>
      </>
   );
}
