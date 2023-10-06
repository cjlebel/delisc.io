'use client';

import React, { useEffect, useState } from 'react';
import { usePathname } from 'next/navigation';

import Link from 'next/link';

import styles from './PopularRelatedTags.module.scss';

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

const TITLE_POPULAR = 'Popular Tags';
const TITLE_RELATED = 'Related Tags';

const PopularRelatedTags = (props: PopularRecentTagsProps) => {
   const [data, setData] = useState<TagResult[] | null>(null);
   const [tagPath, setTagPath] = useState<string>('');
   const [title, setTitle] = useState<string>('');
   const [isLoading, setLoading] = useState<boolean>(true);
   const [error, setError] = useState<any>(null);

   // Get the pathName from the url
   const originalPathName = usePathname();

   const pathName = originalPathName === '/' ? '' : originalPathName.replace('/links/tags/', '');
   const count = props.count && props.count > 0 ? props.count : 25;

   // NOTE: This is called twice. Apparently it's because strict mode is true (next.config.js: reactStrictMode)
   useEffect(() => {
      const tagPath = pathName.replaceAll('+', ' ').replaceAll('/', ',');
      setTagPath(tagPath);

      //TODO: Move this call somewhere else, where if can share a common calling ();
      fetch(`/api/links/tags?tags=${encodeURIComponent(tagPath)}&count=${count}`, {})
         .then((res) => res.json())
         .then((data) => {
            const title = pathName?.replace('/', '').length > 0 ? TITLE_RELATED : TITLE_POPULAR;
            //const d = data as TagResult[];
            setData(data);
            setTitle(title);
            setLoading(false);
         })
         .catch((err) => {
            console.log(err);
            setError(err);
         });
   }, [count, pathName]);

   if (isLoading) return <p>Loading...</p>;
   if (!data) return <p>Failed to load Tags</p>;

   if (error) return <p>Failed to load Tags: {error}</p>;

   if (!data) {
      return <p>Loading Tags....</p>;
   }

   const maxWeight = Math.max(...data.map((tag) => tag.weight));

   const tagItems = data
      ? data.map((tag, idx) => {
           const tagSize =
              tag.weight / maxWeight >= 0.0001 ? (tag.weight / maxWeight) * 1 + 1 : 0.8;
           let colorOption =
              (idx <= colorOptions.length
                 ? colorOptions[idx]
                 : colorOptions[idx % colorOptions.length]) ?? 'bg-white text-dark';

           const tagQuery = `${tagPath}&${tag.name.replaceAll(' ', '+')}`;

           const href = `?tags=${tagQuery}`;

           return (
              <li
                 key={tag.name}
                 style={{ display: 'inline-block', marginRight: '10px', marginBottom: '10px' }}>
                 <Link
                    href={href}
                    className={colorOption}
                    data-count={tag.count}
                    data-weight={tag.weight}
                    data-percent={tag.weight / maxWeight}
                    style={{
                       fontSize: `${tagSize}rem`,
                       padding: '5px',
                       borderRadius: '0.5rem',
                       wordBreak: 'break-word',
                    }}>
                    {tag.name} <span>[{tag.count}]</span>
                 </Link>
                 &nbsp;
              </li>
           );
        })
      : null;

   return (
      <div className='side-panel'>
         <h4 className={`${styles['title']} title`}>{title}</h4>
         <div className={`${styles['contents']} contents`}>
            {tagPath}
            <ul className='list-unstyled'>{tagItems}</ul>
         </div>
      </div>
   );
};

/**
 * PopularRecentTagsProps
 * @typedef PopularRecentTagsProps
 * @property {string} baseApi: The api's base url so that we can retrieve the tags.
 * At the moment it's needed as client-side can't read .env files
 * @property {number} [count=25]: The number of tags to retrieve
 */
type PopularRecentTagsProps = {
   count?: number | 25;
};

export default PopularRelatedTags;
