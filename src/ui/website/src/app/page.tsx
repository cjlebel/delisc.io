import { Suspense } from 'react';

import styles from './page.module.scss';

import { apiGetLinks } from '@/apis';

import LinkCards from '@/components/elements/links/LinkCards';
import { PopularRelatedTags } from '@/components/PopularRelatedTags';
import { Pager } from '@/components/elements/navigation';

export default async function Home({
   searchParams,
}: {
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   const page = searchParams?.page ? parseInt(searchParams.page as string) : 1;
   const tags = searchParams?.tags ? searchParams?.tags.toString().split(',') : [''];

   const links = await apiGetLinks({
      page: page,
      tags: tags,
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
               <PopularRelatedTags maxTags={25} currentTags={tags} />
            </Suspense>
         </aside>
      </>
   );
}
