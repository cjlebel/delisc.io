'use client';

import React, { useEffect, useState } from 'react';

import styles from './TagCloud.module.scss';

import { TagResult } from '@/types/tags';
import { TagPill } from '../TagPill';

const TagCloud = ({ maxTags, currentTags }: TagCloudProps) => {
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

      let tagAsStringForApiCall = trimmedTags.join(',');
      tagAsStringForApiCall = tagAsStringForApiCall.replaceAll('%2B', '%20').replaceAll('+', '%20');

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

           const slug = `/tags/${newTagsArr.join('/').replaceAll('%2B', '+')}`;

           return (
              <TagPill
                 key={`${tag.name}-${idx}`}
                 name={tag.name}
                 className={tagId}
                 href={slug}
                 count={tag.count}
                 totalCount={totalCount}
              />
           );
        })
      : null;

   return <div className={styles.root}>{tagItems}</div>;
};

type TagCloudProps = {
   maxTags?: number | 50;
   currentTags?: string[];
};

export default TagCloud;
