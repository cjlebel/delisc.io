import React from 'react';

import { API_URL } from '@/utils/Configs';

type GetLinksProps = {
   page?: number | 1;
   count?: number | 25;
   search?: string | '';
   tags?: string[] | [];
};

/**
 *
 * @param params { page?: number | 1; count?: number | 25; search?: string | ''; tags?: string[] | [];}
 * @returns SERVER SIDE API: Gets a set of links for the provided parameters
 */
const apiGetLinks = async (params: GetLinksProps) => {
   const page = `page=${params.page ?? 1}`;
   const count = `count=${params.count ?? 25}`;
   const search = `search=${params.search ?? ''}`;
   const tags = `tags=${params.tags ?? ''}`;

   const qs = [page, count, search, tags].filter((x) => x).join('&');

   return await await fetch(`${API_URL}/links?${qs}`);
};

export { apiGetLinks };
