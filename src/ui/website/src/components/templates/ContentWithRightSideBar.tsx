import React from 'react';
import MainContent from './partials/MainContent';
import RightSideContent from './partials/RightSideContent';

type ContentWithRightSideBarProps = {
   main: React.ReactNode;
   rightSide: React.ReactNode;
};

export default function ContentWithRightSideBar({ main, rightSide }: ContentWithRightSideBarProps) {
   return (
      <div className='container-fluid my-2'>
         <div className='d-flex'>
            <MainContent>{main}</MainContent>

            <RightSideContent>{rightSide}</RightSideContent>
         </div>
      </div>
   );
}
