import ApiHelper from '../../../../apis/api-helpers-async';
import { ApiResponse, ApiDataResponse } from '../../../../apis/api-response';

import { EditLinkRequest } from './editLinkRequest';

const errorMessage = 'An error occurred while deleting the links';

class LinkDetailsApis extends ApiHelper {
    /**
     * Deletes a collection of links
     * @param ids The ids of the links to delete
     * @param antiForgeryToken
     * @param onSuccess The callback function to execute when the delete is successful
     * @param onFailure The callback function to execute when the delete fails
     * @returns
     */
    public async updateLinkAsync(link: EditLinkRequest,
        antiForgeryToken: string,
        onSuccess: Function,
        onFailure: Function
    ): Promise<{ isSuccess: boolean; message: string }> {

        if (link.linkId) {
            console.log('Cannot update Link: No id was provided');
        }

        const data = JSON.stringify(link);

        return this.post(`/links/${link.linkId}/edit`, link, antiForgeryToken)
            .then((response) => {
                if (response.isSuccess) {
                    onSuccess(response);
                } else {
                    onFailure(response.message ?? errorMessage);
                }
            })
            .catch((error:Error) => onFailure(error));
    }

    /**
     * Deletes a single link
     * @param linkIds The ids of the links to delete
     * @param antiForgeryToken
     * @param onSuccess The callback function to execute when the delete is successful
     * @param onFailure The callback function to execute when the delete fails
     * @returns
     */
    public async deleteLinkAsync(
        linkId: string,
        antiForgeryToken: string,
        onSuccess: Function,
        onFailure: Function
    ): Promise<ApiDataResponse<string[]>> {
        if (!(linkId?.trim())) {
            onFailure('No id was provided');
        }

        return this.delete(`/links/${linkId}/delete`, antiForgeryToken)
            .then((response) => {
                if (response.isSuccess) {
                    onSuccess((response));
                } else {
                    onFailure(response.message ?? errorMessage);
                }
            })
            .catch((error) => onFailure(error));
    }

    /**
     * Deletes a single link
     * @param linkIds The ids of the links to delete
     * @param antiForgeryToken
     * @param onSuccess The callback function to execute when the delete is successful
     * @param onFailure The callback function to execute when the delete fails
     * @returns
     */
    public async unDeleteLinkAsync(
        linkId: string,
        antiForgeryToken: string,
        onSuccess: Function,
        onFailure: Function
    ): Promise<ApiResponse> {
        if (!(linkId?.trim())) {
            onFailure('No id was provided');
        }

        return this.post(`/links/${linkId}/undelete`, null, antiForgeryToken)
            .then((response : ApiResponse) => {
                if (response.isSuccess) {
                    onSuccess({
                        isSuccess: true,
                        message: `The link '${linkId} was Undeleted successfully'`,
                    });
                } else {
                    onFailure(response.message ?? errorMessage);
                }
            })
            .catch((error) => onFailure(error));
    }
}

export default LinkDetailsApis;
