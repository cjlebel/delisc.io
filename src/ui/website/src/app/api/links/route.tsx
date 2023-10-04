import { NextRequest, NextResponse } from 'next/server';

// type GetLinksRequest = {
//    search?: string | '';
//    tags?: string[] | [];
//    page?: number | 1;
//    count?: number | 30;
// };

export async function GET(request: NextRequest, response: NextResponse) {
   //    const { searchParams } = new URL(request.url);
   //    const pageNo = searchParams.get('pageNo') ? parseInt(searchParams.get('pageNo') ?? '1') : 1;
   //    const count = searchParams.get('count') ? parseInt(searchParams.get('count') ?? '25') : 25;
   //    const search = searchParams.get('search') ?? '';
   //    const tags = searchParams.get('tags') ?? '';
   const searchParams = request.nextUrl.searchParams;

   const page = parseInt(searchParams.get('page') ?? '1');
   const count = parseInt(searchParams.get('count') ?? '25');
   const search = searchParams.get('search') ?? '';
   const tags = searchParams.get('tags') ?? '';

   //onst qs = [page, count, search, tags].filter((x) => x).join('&');

   //    const res = await fetch(`${process.env.REACT_APP_API_URL}/${process.env.REACT_APP_API_VERSION}/links?${qs}`, {
   //       headers: {
   //          'Content-Type': 'application/json',
   //       },
   //    });

   //    //const data: ResultsPage<LinkResult> = await res.json();
   //    return await res.json();

   return NextResponse.json(
      { message: `Page: ${page}, Count: ${count}; Search: ${search}; Tags: ${tags}` },
      { status: 200 }
   );
}
