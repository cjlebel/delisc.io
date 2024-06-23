/**
 * Interface representing a basic API response object with isSuccess and message
 */
export interface ApiResponse {
    isSuccess: boolean;
    message: string | null;
}

export interface ApiDataResponse<T> extends ApiResponse {
    data: T | null;
}

/*
    export interface ApiDataResponse<T> {
    isSuccess: boolean;
    message: string | null;
    data: T | null;
    }
*/