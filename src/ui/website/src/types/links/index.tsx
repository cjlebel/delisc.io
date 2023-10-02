export type LinkResult = {
   id: string;
   description?: string;
   domain?: string;
   imageUrl?: string;
   keywords?: string[] | [];
   tags?: any[] | [];
   title: string;
   url: string;
   submittedById: string;
   dateCreated: string;
   dateUpdated: string;
};

export type LinkItemResult = {
   id: string;
   description: string;
   domain: string;
   imageUrl: string;
   tags?: any[] | [];
   title: string;
   url: string;
};
