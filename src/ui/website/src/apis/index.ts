import { ResultsPage } from '@/types/ResultsPage';
import { LinkResult } from '@/types/links';
import { API_URL } from '@/utils/Configs';

/**
 * Gets a set of links for the provided parameters
 * @param params { page?: number | 1; count?: number | 25; search?: string | ''; tags?: string[] | [];}
 * @returns SERVER SIDE API: Gets a set of links for the provided parameters
 */
const apiGetLinks = async ({
   page,
   count,
   search,
   tags,
}: GetLinksProps) /*: Promise<ResultsPage<LinkResult>>*/ => {
   page = page && page >= 1 ? page : 1;

   // Attempt to get the max number of links, from the .env file
   const maxLinks = process.env.REACT_APP_MAX_LINKS_PER_PAGE ?? '1';
   count = count && count >= 1 ? count : parseInt(maxLinks);

   const tagsAsString = tags ? decodeURIComponent(tags.join(',')).replaceAll('+', ' ') : '';

   var query = new URLSearchParams();
   query.append('page', page.toString());
   query.append('count', count.toString());
   query.append('search', search ?? '');
   query.append('tags', tagsAsString ?? '');

   var data = await fetch(`${API_URL}/links?${query.toString()}`).then((res) => res.json());

   return data as ResultsPage<LinkResult>;
};

/**
 * Gets a set of tags for the provided parameters
 * @param params { tags?: string[] | []; count?: number | 25; }
 * @returns SERVER SIDE API: Gets a set of links for the provided parameters
 */
const apiGetTags = async (params: GetTagsProps) => {
   const tagsAsString = params.tags
      ? decodeURIComponent(params.tags.join(',')).replaceAll('+', ' ')
      : '';

   var query = new URLSearchParams();
   query.append('count', params.count?.toString() ?? '50');
   query.append('tags', tagsAsString ?? '');

   var data = await fetch(`${API_URL}/links/tags?${query.toString()}`, {
      next: { revalidate: 10 },
   });

   if (data.ok) {
      return await data.json();
   }
};

type GetLinksProps = {
   page?: number | 1;
   count?: number | 25;
   search?: string | '';
   tags?: string[] | [];
};

type GetTagsProps = {
   tags?: string[] | [];
   count?: number | 50;
};

export { apiGetLinks, apiGetTags };
