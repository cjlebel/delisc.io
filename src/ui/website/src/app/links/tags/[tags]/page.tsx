import React from 'react';

type LinkTagsPageProps = {
   tags: string;
};

export default function LinkTagsPage({
   params,
}: {
   params: LinkTagsPageProps;
}) {
   return <div>LinkTagsPage: {params.tags}</div>;
}
