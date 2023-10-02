import React, { Suspense } from 'react';
import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { TagResult } from '@/types/tags';
import { LinkCards } from '@/components/elements/links';
import TagsCard from '@/components/elements/tags';
import { Pager } from '@/components/elements/pager';

type LinksTagsPageProps = {
   tags: string;
   pageNo?: number | 1;
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

export default async function LinksTagsPage({
   params,
   searchParams,
}: {
   params: LinksTagsPageProps;
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   //    const searchParams = useSearchParams();
   const pageNo = searchParams?.page ? parseInt(searchParams.page as string) : 1;

   const tagsStr = params.tags ? decodeURIComponent(params.tags).replace('+', ' ') : '';

   const linksData: ResultsPage<LinkResult> = await getTaggedLinks(params.tags, pageNo, 50);
   const tagsData: TagResult[] = await getRelatedTags(params.tags, 50);

   if (!linksData) {
      return <div>Links not found</div>;
   }

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
               <TagsCard preexisting={tagsStr} title='Related Tags' tags={tagsData} />
            </Suspense>
         </aside>
      </>
   );
}
