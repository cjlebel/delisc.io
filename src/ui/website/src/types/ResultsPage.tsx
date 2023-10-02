export type ResultsPage<Type> = {
   results: Type[];
   pageNumber: number;
   pageSize: number;
   totalResults: number;
   totalPages: number;
   isError: boolean;
   isSuccess: boolean;
   message: string;
};
