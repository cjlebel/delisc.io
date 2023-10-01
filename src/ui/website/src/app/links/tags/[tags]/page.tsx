import React, { Suspense } from 'react';
import Link from 'next/link';

import styles from './page.module.scss';

import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { Tag } from '@/types/tags';
import { LinkCards } from '@/components/elements/links';
import TagsCard from '@/components/elements/tags';

type LinksTagsPageProps = {
   tags: string;
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

export default async function LinksTagsPage({ params }: { params: LinksTagsPageProps }) {
   const tagsStr = params.tags ? decodeURIComponent(params.tags).replace('+', ' ') : '';
   const tagsArr = tagsStr ? decodeURIComponent(tagsStr).split(',') : [];

   const linksData: ResultsPage<LinkResult> = await getTaggedLinks(params.tags, 1, 50);
   const tagsData: Tag[] = await getRelatedTags(params.tags, 50);

   const tags = tagsArr.map((tag: string) => {
      return <span key={tag}>{tag} </span>;
   });

   return (
      <>
         <div>
            <Link href='/' title='Home'>
               Home
            </Link>{' '}
            / {tags}
         </div>
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
               <TagsCard preexisting={tagsStr} title='Related Tags' tags={tagsData} />
            </Suspense>
         </aside>
      </>
   );
}
