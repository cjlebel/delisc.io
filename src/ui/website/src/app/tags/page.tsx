import React from 'react';

import { TagCloud } from '@/components/TagCloud';

type TagsPageProps = {};

export default function TagsPage({}: TagsPageProps) {
   return (
      <div>
         <h1>Tags</h1>
         <div>
            <TagCloud maxTags={200} />
         </div>
      </div>
   );
}
