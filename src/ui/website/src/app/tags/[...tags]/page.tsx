import LinksPage from '@/components/modules/LinksPage/LinksPage';

export default async function TagsLinksPage({
   params,
   searchParams,
}: {
   params: { tags: string[] };
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   console.log('Tags Page');
   return <LinksPage tagsParams={params.tags} searchParams={searchParams} />;
}
