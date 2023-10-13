import React from 'react';

const MainContent = ({ children }: { children: React.ReactNode }) => {
   return <div className='flex-grow-1 me-4'>{children}</div>;
};

export default MainContent;
