'use client';
import React from 'react';
import RightSidePanel from '../RightSidePanel/RightSidePanel';

type RelatedLinksPanelProps = {
   tags: string[];
};

export default function RelatedLinksPanel({ tags }: RelatedLinksPanelProps) {
   return (
      <RightSidePanel title='Related Links'>
         <div>
            <h4>Link 1</h4>
            <h4>Link 2</h4>
            <h4>Link 3</h4>
            <h4>Link 4</h4>
            <h4>Link 5</h4>
            <h4>Link 6</h4>
            <h4>Link 7</h4>
            <h4>Link 8</h4>
            <h4>Link 9</h4>
         </div>
      </RightSidePanel>
   );
}
