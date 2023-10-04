import { API_URL } from '@/utils/Configs';

/**
 * Gets a set of links for the provided parameters
 * @param params { page?: number | 1; count?: number | 25; search?: string | ''; tags?: string[] | [];}
 * @returns SERVER SIDE API: Gets a set of links for the provided parameters
 */
const apiGetLinks = async (params: GetLinksProps) => {
   const tagsAsString = params.tags
      ? decodeURIComponent(params.tags.join(',')).replaceAll('+', ' ')
      : '';

   const count = `count=${params.count ?? 25}`;
   const page = `page=${params.page ?? 1}`;
   const search = `search=${params.search ?? ''}`;
   const tags = `tags=${tagsAsString ?? ''}`;

   const qs = [page, count, search, tags].filter((x) => x).join('&');

   return await await fetch(`${API_URL}/links?${qs}`);
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

   const count = `count=${params.count ?? 50}`;
   const tags = `tags=${tagsAsString ?? ''}`;

   const qs = [tags, count].filter((x) => x).join('&');

   var data = await fetch(`${API_URL}/links/tags?${qs}`, {
      next: { revalidate: 60 },
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
