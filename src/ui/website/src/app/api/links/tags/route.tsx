import { NextRequest, NextResponse } from 'next/server';

const API_KEY = process.env.REACT_APP_API_KEY;
const USER_AGENT = 'deliscio-web-client';

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
         method: 'GET',
         headers: {
            'x-api-key': `${API_KEY}`,
            'User-Agent': `${USER_AGENT}`,
            'Content-Type': 'application/json',
            //Accept: '*/*',
         },
      }
   );

   //    //const data: ResultsPage<LinkResult> = await res.json();
   return await res;
}
