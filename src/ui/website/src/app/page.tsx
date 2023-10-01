import { Suspense, cache } from 'react';

import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { Tag } from '@/types/tags';

import LinkCards from '@/components/elements/links/LinkCards';
import PopularTagsCard from '@/components/elements/tags';

const getLinks = async (pageNo: number, size: number) => {
   var data = await fetch(`${API_URL}/links/${pageNo}/${size}`);

   if (data.ok) {
      return await data.json();
   }
};

const getTopTags = async (size: number) => {
   var data = await fetch(`${API_URL}/links/tags/top/${size}`, { next: { revalidate: 10 } });

   if (data.ok) {
      return await data.json();
   }
};

export default async function Home() {
   var linksData: ResultsPage<LinkResult> = await getLinks(1, 50);
   var tagsData: Tag[] = await getTopTags(50);

   return (
      <>
         <section className={styles.content}>
            <Suspense fallback={<>Loading...</>}>
               <LinkCards items={linksData.results} />
            </Suspense>
            <div>
               Page {linksData.pageNumber} of {linksData.totalPages} ({linksData.totalResults}{' '}
               Results)
            </div>
         </section>
         <aside className={styles.sidebar}>
            <Suspense fallback={<>Loading...</>}>
               <PopularTagsCard title='Popular Tags' tags={tagsData} />
            </Suspense>
         </aside>
      </>
   );
}
