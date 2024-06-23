import { ApiResponse, ApiDataResponse } from './api-response';

/**
 * Define a union type for the HTTP methods
 */
type HttpMethod = 'GET' | 'PATCH' | 'POST' | 'PUT' | 'DELETE';

/**
 * API Helper class for making API requests
 */
class ApiHelper {
    /**
     * Helper function to make API requests
     * @param {HttpMethod} method - The HTTP method (GET, POST, PUT, DELETE)
     * @param {string} url - The URL for the API endpoint
     * @param {Object} data - The data to be sent in the request body
     * @param {Object} params - Query parameters (for GET)
     * @returns {Promise<any>} - Promise that resolves with the API response object
     */
    private async apiRequest<T extends ApiResponse | ApiDataResponse<any>>(
        method: HttpMethod,
        url: string,
        data: any = null,
        params: any = null,
        antiForgeryToken: string | null = null
    ): Promise<T> {
        const headers: HeadersInit = new Headers();
        headers.append('Content-Type', 'application/json');
        if (antiForgeryToken && antiForgeryToken.length > 0) {
            headers.append('RequestVerificationToken', antiForgeryToken);
        }

        if (params) {
            const queryParams = new URLSearchParams(params).toString();
            url += `?${queryParams}`;
        }

        let bodyData = data ? JSON.stringify(data) : null;
        try {
            const response = await fetch(url, {
                method,
                headers,
                body: bodyData,
            });

            const contentType = response.headers.get('Content-Type');
            const isJson = contentType && contentType.includes('application/json');

            if (isJson) {
                const responseData = await response.json();

                if (this.isApiDataResponse<T>(responseData)) {
                    const apiDataResponse = {
                        isSuccess: response.ok,
                        message: response.ok ? null : `${response.status} ${response.statusText}`,
                        data: responseData,
                    } as T;

                    return apiDataResponse;

                } else {
                    const apiResponse = {
                        isSuccess: response.ok,
                        message: response.ok ? null : `${response.status} ${response.statusText}`,
                    } as T;

                    return apiResponse;
                }
            } else {
                const apiResponse = {
                    isSuccess: response.ok,
                    message: response.ok ? null : `${response.status} ${response.statusText}`,
                } as T;

                return apiResponse;

                //return {
                //    isSuccess: response.ok,
                //    message: response.ok ? null : `${response.status} ${response.statusText}`,
                //} as T;
                //if (this.isApiDataResponse<T>(null)) {
                //    return {
                //        isSuccess: response.ok,
                //        message: response.ok
                //            ? null
                //            : `${response.status} ${response.statusText}. Expected JSON response.`,
                //        data: null,
                //    } as T;
                //} else {
                //    return {
                //        isSuccess: response.ok,
                //        message: response.ok ? null : `${response.status} ${response.statusText}`,
                //    } as T;
                //}
            }
        } catch (error) {
            return {
                isSuccess: false,
                message: `API request failed: ${(error as Error).message}`,
            } as T;
        }
    }

    private isApiDataResponse<T>(value: any): value is ApiDataResponse<T> {
        return (value as ApiDataResponse<T>).data !== undefined;
    }

    /**
     * Helper function for GET requests
     * @param {string} url - The URL for the API endpoint
     * @param {Object} params - Query parameters
     * @returns {Promise<T>} - Promise that resolves with the API response object
     */
    public async get<T extends ApiDataResponse<any>>(
        url: string,
        params?: any
    ): Promise<T> {
        return this.apiRequest<T>('GET', url, null, params);
    }

    /**
     * Helper function for POST requests
     * @param {string} url - The URL for the API endpoint
     * @param {Object} data - The data to be sent in the request body
     * @returns {Promise<any>} - Promise that resolves with the API response object
     */
    public async post<T extends ApiResponse | ApiDataResponse<any>>(
        url: string,
        data: any,
        antiForgeryToken?: string | null
    ): Promise<T> {
        return this.apiRequest<T>('POST', url, data, null, antiForgeryToken);
    }

    /**
     * Helper function for PATCH requests
     * NOTE: PATCH requests update a small piece of data on the server
     * @param {string} url - The URL for the API endpoint
     * @param {Object} data - The data to be sent in the request body
     * @returns {Promise<ApiResponse | ApiDataResponse<any>>} - Promise that resolves with the API response object
     */
    public async patch<T extends ApiResponse>(
        url: string,
        data: any,
        antiForgeryToken?: string | null
    ): Promise<T> {
        return this.apiRequest<T>('PATCH', url, data, null, antiForgeryToken);
    }

    /**
     * Helper function for PUT requests
     * NOTE: PUT requests will update existing data if it exists, or create new data if it does not
     * @param {string} url - The URL for the API endpoint
     * @param {Object} data - The data to be sent in the request body
     * @returns {Promise<any>} - Promise that resolves with the API response object
     */
    public async put<T extends ApiResponse>(
        url: string,
        data: any,
        antiForgeryToken?: string | null
    ): Promise<T> {
        return this.apiRequest<T>('PUT', url, data, null, antiForgeryToken);
    }

    /**
     * Helper function for DELETE requests, to delete a single record
     * @param {string} url - The URL for the API endpoint
     * @param {string} antiForgeryToken - The anti-forgery token to be sent in the request header (if applicable)
     * @returns {Promise<any>} - Promise that resolves with the API response object
     */
    public async delete<T extends ApiResponse>(
        url: string,
        data: any,
        antiForgeryToken?: string | null
    ): Promise<T> {
        return this.apiRequest<T>('DELETE', url, data, null, antiForgeryToken);
    }

    /**
     * Helper function for DELETE requests, to delete more than one record
     * @param {string} url - The URL for the API endpoint
     * @param {Object} data - The data to be sent in the request body
     * @param {string} antiForgeryToken - The anti-forgery token to be sent in the request header (if applicable)
     * @returns {Promise<any>} - Promise that resolves with the API response object
     */
    public async deletes<T extends ApiResponse>(
        url: string,
        data: any,
        antiForgeryToken: string | null
    ): Promise<T> {
        return this.apiRequest<T>('DELETE', url, data, null, antiForgeryToken);
    }
}

// Export the ApiHelper class
export default ApiHelper;
