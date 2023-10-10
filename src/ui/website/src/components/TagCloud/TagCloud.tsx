'use client';

import React, { useEffect, useState } from 'react';

import Link from 'next/link';

import styles from './TagCloud.module.scss';

import { TagResult } from '@/types/tags';
import { TagPill } from '../TagPill';

const colorOptions = [
   'bg-deliscio',
   'bg-primary',
   'bg-secondary',
   'bg-success',
   'bg-warning text-dark',
   'bg-info text-dark',
   //    'bg-light text-dark',
   'bg-deliscio text-dark',
   'bg-primary text-dark',
   'bg-secondary text-dark',
   'bg-success text-dark',
];

const TagCloud = ({ maxTags, currentTags }: PopularRecentTagsProps) => {
   const [tags, setTags] = useState<TagResult[] | null>(null);
   const [tagsToFilterBy, setCurrentTags] = useState<string[]>(['']);
   const [isLoading, setLoading] = useState<boolean>(true);
   const [error, setError] = useState<any>(null);

   // NOTE: This is called twice. Apparently it's because strict mode is true (next.config.js: reactStrictMode)
   useEffect(() => {
      // Sort the tags for consistency
      var trimmedTags = currentTags
         ? currentTags.filter((t) => {
              return t.trim() !== '';
           })
         : [];

      setCurrentTags(trimmedTags);

      const tagAsStringForApiCall = trimmedTags.join(',').replaceAll('+', ' ');

      //TODO: Move this call somewhere else, where if can share a common calling ();
      fetch(
         `/api/links/tags?tags=${encodeURIComponent(tagAsStringForApiCall)}&count=${maxTags}`,
         {}
      )
         .then((res) => res.json())
         .then((data) => {
            setTags(data);
            setLoading(false);
         })
         .catch((err) => {
            console.log(err);
            setError(err);
         });
   }, [maxTags, currentTags]);

   if (isLoading) return <p>Loading...</p>;

   if (error) return <p>Failed to load Tags: {error}</p>;

   if (!tags) {
      return <></>;
   }

   let totalCount = 0;

   tags.forEach((tag) => {
      totalCount += tag.count;
   });

   const tagItems = tags
      ? tags.map((tag, idx) => {
           const tagId = `tag-${(idx % 10) + 1}`;

           const tagName = tag.name.replaceAll(' ', '+').replaceAll('%20', '+');
           //TODO: Rearrange the tags in the querystring by the count, so most popular is always first
           //      This would mean adding the existing tags back into the results that are returned so that we can get the count
           //      Then filtering them out of what gets displayed
           //      This is so that there'll be consistency.
           //TODO: This can be better. Reuse array from above.

           let newTagsArr =
              [...tagsToFilterBy, tagName]
                 ?.filter((t, idx) => {
                    return t.toString().trim() !== '';
                 })
                 .sort() ?? [];

           const href = `/tags/${newTagsArr.join('/')}`;

           return (
              <>
                 <TagPill
                    name={tag.name}
                    className={styles[tagId]}
                    href={href}
                    count={tag.count}
                    totalCount={totalCount}
                 />
                 {/* <Link
                    href={href}
                    className={`${styles.tag} ${}`}
                    key={tag.name}
                    data-count={tag.count}
                    data-weight={tag.weight}
                    data-totalcount={totalCount}
                    data-percent={tag.count / totalCount}
                    style={{
                       fontSize: `${tagSize}rem`,
                    }}>
                    <span>
                       {tag.name}
                    </span>
                 </Link> */}
              </>
           );
        })
      : null;

   return (
      <div className={styles.root}>
         <ul className='list-unstyled'>{tagItems}</ul>
      </div>
   );
};

/**
 * PopularRecentTagsProps
 * @typedef TagCloudProps
 * @property {string} baseApi: The api's base url so that we can retrieve the tags.
 * At the moment it's needed as client-side can't read .env files
 * @property {number} [count=50]: The number of tags to retrieve
 */
type PopularRecentTagsProps = {
   maxTags?: number | 50;
   currentTags?: string[] | undefined;
};

export default TagCloud;
