'use client';

import React, { useEffect } from 'react';
import { usePathname } from 'next/navigation';
import useSWR from 'swr';

import Link from 'next/link';

import styles from './Tags.module.scss';

//import { API_URL } from '@/utils/Configs';

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

const fetcher = (url: string) =>
   fetch(url).then((res) => {
      return res.json();
   });

const PopularRecentTags = (props: PopularRecentTagsProps) => {
   /* //Was going to use this, but, the component, although it's client side, renders per page load.
   const [title, setTitle] = useState('Popular Tags');
   const [path, setPath] = useState('');

   const originalPathName = usePathname();

   useEffect(() => {
      const p = originalPathName.replace('links/tags/', '');
      const t = !p ? 'Popular Tags' : 'Related Tags';

      setPath(p);
      setTitle(t);
   }, [originalPathName]);
    */

   // Get the pathName from the url
   const originalPathName = usePathname();
   // TODO: May want to check querystring too? (i.e. ?t=tags1,tag2,tag+3)

   const pathName = originalPathName.replace('/links/tags/', '');
   const title = pathName?.replace('/', '').length > 0 ? TITLE_RELATED : TITLE_POPULAR;

   const tags =
      pathName.length > 0
         ? encodeURIComponent(pathName.replaceAll('+', ' ').replaceAll('/', ','))
         : '';

   const count = props.count && props.count > 0 ? props.count : 25;

   const api =
      title === TITLE_POPULAR
         ? `${props.baseApi}/links/tags/top/${count}`
         : `${props.baseApi}/links/tags/${tags}`;

   const { data, error } = useSWR(api, fetcher);

   if (error) {
      return (
         <>
            <h1>{title}</h1>
            <p>
               Failed to load Tags: <br />
               API:{api}
               <br />
               Error:{error.toString()}
               <br />
               Data:{data}
               <br />
            </p>
         </>
      );
   }
   if (!data) {
      return <p>Loading Tags....</p>;
   }

   var response = data as TagResult[];

   const maxWeight = Math.max(...response.map((tag) => tag.weight));

   const tagItems = response
      ? response.map((tag, idx) => {
           const tagSize =
              tag.weight / maxWeight >= 0.0001 ? (tag.weight / maxWeight) * 1 + 1 : 0.8;
           let colorOption =
              (idx <= colorOptions.length
                 ? colorOptions[idx]
                 : colorOptions[idx % colorOptions.length]) ?? 'bg-white text-dark';

           const href = `/links/tags/${pathName}/${tag.name.replaceAll(' ', '+')}`;
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
                    {tag.name}{' '}
                    <span
                       style={
                          colorOption == 'bg-white text-dark'
                             ? {
                                  fontSize: '0.5em',
                                  verticalAlign: 'super',
                                  color: 'black',
                                  fontWeight: 'bold',
                               }
                             : {
                                  fontSize: '0.5em',
                                  verticalAlign: 'super',
                                  color: 'white',
                                  fontWeight: 'bold',
                               }
                       }>
                       [{tag.count}]
                    </span>
                 </Link>
                 &nbsp;
              </li>
           );
        })
      : null;

   return (
      <div className={`card ${styles.card}`} style={{ width: '100%' }}>
         <div className={`card-header ${styles['card-header']}`}>{title}</div>
         <div className='card-body'>
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
   baseApi: string;
   count?: number | 25;
};

export default PopularRecentTags;
