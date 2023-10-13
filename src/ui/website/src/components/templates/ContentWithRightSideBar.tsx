import React from 'react';
import MainContent from './partials/MainContent';
import RightSideContent from './partials/RightSideContent';

type ContentWithRightSideBarProps = {
   main: React.ReactNode;
   rightSide: React.ReactNode;
};

export default function ContentWithRightSideBar({ main, rightSide }: ContentWithRightSideBarProps) {
   return (
      <div className='d-flex' style={{ width: '100%' }}>
         <MainContent>{main}</MainContent>

         <RightSideContent>{rightSide}</RightSideContent>
      </div>
   );
}
