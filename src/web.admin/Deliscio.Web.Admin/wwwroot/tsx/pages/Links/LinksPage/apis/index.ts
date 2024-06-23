import ApiHelper from '../../../../apis/api-helpers-async';
import { ApiDataResponse, ApiResponse } from '../../../../apis/api-response';

const errorMessage = 'An error occurred while deleting the links';

class LinksApis extends ApiHelper {
    /**
     * Deletes a collection of links
     * @param linkIds The ids of the links to delete
     * @param antiForgeryToken
     * @param onSuccess The callback function to execute when the delete is successful
     * @param onFailure The callback function to execute when the delete fails
     * @returns
     */
    public async deleteLinksAsync(
        linkIds: string[],
        antiForgeryToken: string,
        onSuccess: Function,
        onFailure: Function
    ): Promise<ApiDataResponse<string[]>> {
        if (!linkIds || linkIds.length === 0) {
            onFailure('No ids provided');
        }

        return this.deletes<ApiDataResponse<string[]>>(`/links/deletes`, linkIds, antiForgeryToken)
            .then((response) => {
                if (response.isSuccess) {
                    onSuccess(response.data);
                } else {
                    onFailure(response.message ?? errorMessage);
                }
            })
            .catch((error) => onFailure(error));
    }
}

export default LinksApis;
