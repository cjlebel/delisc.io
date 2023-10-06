import { Suspense } from 'react';

import styles from './page.module.scss';

import { apiGetLinks } from '@/apis';

import LinkCards from '@/components/elements/links/LinkCards';
import { PopularRelatedTags } from '@/components/molecules/PopularRelatedTags';
import { Pager } from '@/components/elements/navigation';

export default async function Home({
   searchParams,
}: {
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   const page = searchParams?.page ? parseInt(searchParams.page as string) : 1;

   const links = await apiGetLinks({
      page: page,
   });

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
               <PopularRelatedTags count={70} />
            </Suspense>
         </aside>
      </>
   );
}
