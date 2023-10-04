import { NextRequest, NextResponse } from 'next/server';

export async function GET(request: NextRequest, response: NextResponse) {
   const searchParams = request.nextUrl.searchParams;

   const count = searchParams.get('count'); //parseInt(searchParams.get('count') ?? '50');
   const tags = searchParams.get('tags') ?? '';

   const tagsAsString =
      tags.length > 0 ? encodeURIComponent(tags.replaceAll('+', ' ').replaceAll('/', ',')) : '';

   const qsCount = `count=${count}`;
   const qsTags = `tags=${tagsAsString}`;

   const qs = [qsTags, qsCount].filter((x) => x).join('&');

   const res = await fetch(
      `${process.env.REACT_APP_API_URL}/${process.env.REACT_APP_API_VERSION}/links/tags?${qs}`,
      {
         headers: {
            'Content-Type': 'application/json',
         },
      }
   );

   //    //const data: ResultsPage<LinkResult> = await res.json();
   return await res;
}
