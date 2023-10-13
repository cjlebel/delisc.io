import { TagResult } from '../tags';

export type LinkResult = {
   id: string;
   description: string;
   domain: string;
   imageUrl?: string;
   keywords?: string[] | [];
   tags?: TagResult[] | [];
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
   tags?: TagResult[] | [];
   title: string;
   url: string;
};
