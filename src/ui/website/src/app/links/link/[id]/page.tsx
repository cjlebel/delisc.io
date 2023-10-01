import React from 'react';

type LinkPageProps = {
   id: string;
};

export default function LinkPage({ params }: { params: LinkPageProps }) {
   return <div>Link Page: {params.id}</div>;
}
