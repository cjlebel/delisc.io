import { Suspense } from 'react';

import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { TagResult } from '@/types/tags';

import LinkCards from '@/components/elements/links/LinkCards';
import TagsCard from '@/components/elements/tags';
import { Pager } from '@/components/elements/pager';

const getLinks = async (pageNo: number, size: number) => {
   var data = await fetch(`${API_URL}/links/${pageNo}/${size}`);

   if (data.ok) {
      return await data.json();
   }
};

const getTopTags = async (size: number) => {
   var data = await fetch(`${API_URL}/links/tags/top/${size}`, { next: { revalidate: 1 } });

   if (data.ok) {
      return await data.json();
   }
};

export default async function Home() {
   const linksData: ResultsPage<LinkResult> = await getLinks(1, 50);
   const tagsData: TagResult[] = await getTopTags(50);

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
               <TagsCard title='Popular Tags' tags={tagsData} />
            </Suspense>
         </aside>
      </>
   );
}
