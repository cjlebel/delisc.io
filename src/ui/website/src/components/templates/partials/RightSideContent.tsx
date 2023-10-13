import React from 'react';

const RightSideContent = ({ children }: { children: React.ReactNode }) => {
   return (
      <div className='flex-shrink-0 pe-4' style={{ width: '30%', maxWidth: '400px' }}>
         {children}
      </div>
   );
};

export default RightSideContent;
