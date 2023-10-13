import { API_URL } from '@/utils/Configs';

import { ResultsPage } from '@/types/ResultsPage';
import { LinkItemResult, LinkResult } from '@/types/links';

/**
 * Utils class for Client Side API calls (so not to confuse them with client side when calling them)
 */
namespace ServerSideClientApis {
   const API_KEY = process.env.REACT_APP_API_KEY;
   const USER_AGENT = 'deliscio-web-client';

   export class LinksApis {
      /**
       * Gets a single link by its id
       * @param {string} id: The id of the link to retrieve
       * @returns A promise with a LinkItemResult
       */
      static async getLink(id: string): Promise<LinkItemResult> {
         if (!id) throw new Error('Link Id is required');

         var data = await fetch(`${API_URL}/link/${id}`, {
            //mode: 'cors',
            headers: {
               'x-api-key': `${API_KEY}`,
               'User-Agent': `${USER_AGENT}`,
               //Accept: '*/*',
            },
            next: { revalidate: 10 },
         }).then((res) => res.json());

         if (data.ok) {
            return await data.json();
         }

         return data as LinkItemResult;
      }

      /**
       *
       * @param {GetLinksProps} params
       * @returns
       */
      static async getLinks({
         page,
         count,
         search,
         tags,
      }: GetLinksProps): Promise<ResultsPage<LinkResult>> {
         page = page && page >= 1 ? page : 1;

         // Attempt to get the max number of links, from the .env file
         const maxLinks = process.env.REACT_APP_MAX_LINKS_PER_PAGE ?? '1';
         count = count && count >= 1 ? count : parseInt(maxLinks);

         const tagsAsString = tags ? decodeURIComponent(tags.join(',')).replaceAll('+', ' ') : '';

         let query = new URLSearchParams();
         query.append('page', page.toString());
         query.append('count', count.toString());
         query.append('search', search ?? '');
         query.append('tags', tagsAsString ?? '');

         let data = await fetch(`${API_URL}/links?${query.toString()}`, {
            //mode: 'cors',
            headers: {
               'x-api-key': `${API_KEY}`,
               'User-Agent': `${USER_AGENT}`,
               //Accept: '*/*',
            },
            next: { revalidate: 10 },
         }).then((res) => res.json());

         return data as ResultsPage<LinkResult>;
      }

      /**
       * Gets a set of tags for the provided parameters
       * @param params { tags?: string[] | []; count?: number | 25; }
       * @returns SERVER SIDE API: Gets a set of links for the provided parameters
       */
      apiGetTags = async (params: GetTagsProps) => {
         const tagsAsString = params.tags
            ? decodeURIComponent(params.tags.join(',')).replaceAll('+', ' ')
            : '';

         let query = new URLSearchParams();
         query.append('count', params.count?.toString() ?? '50');
         query.append('tags', tagsAsString ?? '');

         let data = await fetch(`${API_URL}/links/tags?${query.toString()}`, {
            //mode: 'cors',
            headers: {
               'x-api-key': `${API_KEY}`,
               'User-Agent': `${USER_AGENT}`,
               //Accept: '*/*',
            },
            next: { revalidate: 10 },
         });
         //   .then((response) => {
         //      if (response.ok) {
         //         return response.json();
         //      }
         //      throw new Error('Network response was not ok.');
         //   })
         //   .then((data) => {
         //      // Handle the API response data
         //      console.log(data);
         //   })
         //   .catch((error) => {
         //      // Handle errors
         //      console.error('There has been a problem with your fetch operation:', error);
         //   })
         if (data.ok) {
            return await data.json();
         }
      };
   }
}

/**
 * Properties to get a collection of Links
 */
type GetLinksProps = {
   /**
    * The number of the page of results to return. If null, then 1 will be used
    */
   page?: number | 1;
   /**
    * The max number of results to return
    */
   count?: number | 25;
   search?: string | '';
   tags?: string[] | [];
};

/**
 * Properties to get a collection of Tags
 */
type GetTagsProps = {
   tags?: string[] | [];
   count?: number | 50;
};

export { ServerSideClientApis };
