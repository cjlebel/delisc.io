import { LinksPage } from '@/components/modules/LinksPage';

export default async function Home({
   searchParams,
}: {
   searchParams?: { [key: string]: string | string[] | undefined };
}) {
   return <LinksPage searchParams={searchParams} />;
}
