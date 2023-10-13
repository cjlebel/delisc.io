'use client';

import React, { useEffect, useState } from 'react';

import { TagCloud } from '../TagCloud';
import RightSidePanel from '../RightSidePanel/RightSidePanel';

const TITLE_POPULAR = 'Popular Tags';
const TITLE_RELATED = 'Related Tags';

/**
 *
 * @param {PopularRecentTagsProps}
 * @returns
 */
const PopularRelatedTags = (props: PopularRecentTagsProps) => {
   const [maxTags, setMaxTags] = useState<number>(50);
   const [title, setTitle] = useState<string>('');

   useEffect(() => {
      const existingTags = props.currentTags
         ? props.currentTags
              .filter((t) => {
                 return t.trim() !== '';
              })
              .sort()
         : null;

      // Yes, they are the same. One may potentially have more than the other at a later time
      const maxTags = existingTags && existingTags.length > 0 ? 100 : 100;
      setMaxTags(maxTags);

      const title = existingTags && existingTags.length > 0 ? TITLE_RELATED : TITLE_POPULAR; // tagDelimited?.replace('/', '').length > 0 ? TITLE_RELATED : TITLE_POPULAR;
      setTitle(title);
   }, [props.currentTags]);

   return (
      <RightSidePanel title={title}>
         <TagCloud maxTags={maxTags} currentTags={props.currentTags} />
      </RightSidePanel>
   );
};

/**
 * PopularRecentTagsProps
 * @typedef PopularRecentTagsProps
 * @property {string} baseApi: The api's base url so that we can retrieve the tags.
 * At the moment it's needed as client-side can't read .env files
 * @property {number} [maxTags=50]: The number of tags to retrieve
 */
type PopularRecentTagsProps = {
   currentTags?: string[];
};

export default PopularRelatedTags;
