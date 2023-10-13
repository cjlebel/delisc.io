import { apiGetLinks } from '@/apis';

import LinkCards from '@/components/elements/links/LinkCards';
import { Pager } from '@/components/elements/navigation';
import { PopularRelatedTags } from '@/components/PopularRelatedTags';
import { ContentWithRightSideBar } from '@/components/templates';

type LinksPageProps = {
   tagsParams?: string[];
   searchParams?: { [key: string]: string | string[] | undefined };
};

/**
 * A reusable page component to display a page of links
 */
export default async function LinksPage({ tagsParams, searchParams }: LinksPageProps) {
   const page = searchParams?.page ? parseInt(searchParams.page as string) : 1;
   const tags =
      tagsParams && tagsParams.length > 0
         ? tagsParams
         : searchParams?.tags
         ? searchParams?.tags?.toString().split(',')
         : [];

   const links = await apiGetLinks({
      page: page,
      tags: tags,
   });

   const getMainContent = (): React.ReactNode => {
      return (
         <>
            <LinkCards items={links.results} />
            <Pager
               currentPage={links.pageNumber}
               totalPages={links.totalPages}
               totalResults={links.totalResults}
            />
         </>
      );
   };

   return (
      <>
         <ContentWithRightSideBar
            main={getMainContent()}
            rightSide={<PopularRelatedTags currentTags={tags} />}
         />
      </>
   );
}
