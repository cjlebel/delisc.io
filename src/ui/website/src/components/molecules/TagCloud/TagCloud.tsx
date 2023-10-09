'use client';

import React, { useEffect, useState } from 'react';

import Link from 'next/link';

import styles from './TagCloud.module.scss';

import { TagResult } from '@/types/tags';

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
   const [sortedTags, setCurrentTagsSorted] = useState<string[]>(['']);
   const [isLoading, setLoading] = useState<boolean>(true);
   const [error, setError] = useState<any>(null);

   // NOTE: This is called twice. Apparently it's because strict mode is true (next.config.js: reactStrictMode)
   useEffect(() => {
      // Sort the tags for consistency
      const sortedTags = currentTags
         ? currentTags
              .filter((t) => {
                 return t.trim() !== '';
              })
              .sort()
         : [];

      const tagDelimited = sortedTags.toString().replaceAll('+', ' ');

      setCurrentTagsSorted(sortedTags);

      //TODO: Move this call somewhere else, where if can share a common calling ();
      fetch(`/api/links/tags?tags=${encodeURIComponent(tagDelimited)}&count=${maxTags}`, {})
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

   //const maxWeight = Math.max(...data.map((tag) => tag.weight));

   const tagItems = tags
      ? tags.map((tag, idx) => {
           const tagSize =
              totalCount > 0 && tag.count / totalCount >= 0.0001
                 ? 1 + (tag.count / totalCount) * 5
                 : 0.8;

           const tagId = `tag-${(idx % 10) + 1}`;

           const tagName = tag.name.replaceAll(' ', '+').replaceAll('%20', '+');
           //TODO: Rearrange the tags in the querystring by the count, so most popular is always first
           //      This would mean adding the existing tags back into the results that are returned so that we can get the count
           //      Then filtering them out of what gets displayed
           //      This is so that there'll be consistency.
           //TODO: This can be better. Reuse array from above.
           const newTagsArr = [sortedTags, tagName]
              .filter((t) => {
                 return t.toString().trim() !== '';
              })
              .sort();

           const href = `?tags=${newTagsArr.join(',')}`;

           return (
              <li className={`${styles.tag}`} key={tag.name}>
                 <Link
                    href={href}
                    className={`${styles[tagId]}`}
                    data-count={tag.count}
                    data-weight={tag.weight}
                    data-totalcount={totalCount}
                    data-percent={tag.count / totalCount}
                    style={{
                       fontSize: `${tagSize}rem`,
                       padding: '5px',
                       borderRadius: '0.5rem',
                       wordBreak: 'break-word',
                    }}>
                    <span>
                       {tag.name} <span>[{tag.count}]</span>
                    </span>
                 </Link>
                 &nbsp;
              </li>
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
