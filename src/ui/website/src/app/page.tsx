import { Suspense } from 'react';

import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';
import { apiGetLinks } from '@/apis';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { TagResult } from '@/types/tags';

import LinkCards from '@/components/elements/links/LinkCards';
import { PopularRecentTags } from '@/components/elements/tags';
import { Pager } from '@/components/elements/pager';

const getLinks = async (page: number, count: number) => {
   const data = await apiGetLinks({ page: page, count: count });

   if (data.ok) {
      return await data.json();
   }
};

export default async function Home({
   searchParams,
}: {
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   const page = searchParams?.page ? parseInt(searchParams.page as string) : 1;
   const links: ResultsPage<LinkResult> = await getLinks(page, 27);
   return (
      <>
         <section className={styles.content}>
            <Suspense fallback={<>Loading...</>}>
               <LinkCards items={links.results} />
            </Suspense>
            <Pager
               currentPage={links.pageNumber}
               totalPages={links.totalPages}
               totalResults={links.totalResults}
            />
         </section>
         <aside className={`sidebar ${styles.sidebar}`}>
            <Suspense fallback={<>Loading...</>}>
               <PopularRecentTags count={70} />
            </Suspense>
         </aside>
      </>
   );
}
