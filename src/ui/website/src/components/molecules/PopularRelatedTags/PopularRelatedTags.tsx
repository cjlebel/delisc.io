'use client';

import React, { useEffect, useState } from 'react';

import styles from './PopularRelatedTags.module.scss';

import { TagCloud } from '../TagCloud';

const TITLE_POPULAR = 'Popular Tags';
const TITLE_RELATED = 'Related Tags';

/**
 *
 * @param {PopularRecentTagsProps}
 * @returns
 */
const PopularRelatedTags = (props: PopularRecentTagsProps) => {
   const [title, setTitle] = useState<string>('');

   useEffect(() => {
      const existingTags = props.currentTags
         ? props.currentTags
              .filter((t) => {
                 return t.trim() !== '';
              })
              .sort()
         : null;

      const title = existingTags && existingTags.length > 0 ? TITLE_RELATED : TITLE_POPULAR; // tagDelimited?.replace('/', '').length > 0 ? TITLE_RELATED : TITLE_POPULAR;
      setTitle(title);
   }, [props.maxTags, props.currentTags]);

   return (
      <div className='tags-panel'>
         <h4 className={`${styles['title']} title`}>{title}</h4>
         Current Tags: {props.currentTags?.length} {props.currentTags}
         <div className={`${styles['contents']} contents`}>
            <TagCloud maxTags={props.maxTags} currentTags={props.currentTags} />
         </div>
      </div>
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
   maxTags?: number | 50;
   currentTags?: string[];
};

export default PopularRelatedTags;
