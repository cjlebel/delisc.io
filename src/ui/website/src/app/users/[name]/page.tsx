import React from 'react';

type UserProfileProps = {
   name: string;
};

export default function UserProfile({ params }: { params: UserProfileProps }) {
   return (
      <div>
         UserProfile: {params.name}!!!
         <br />
      </div>
   );
}
