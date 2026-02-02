import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { FilamentListResponse } from '../models/filament/FilamentListResponse';
import type { FilamentRequest } from '../models/filament/FilamentRequest';

export const getFilamentList = async (): Promise<CommonResponse<FilamentListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<FilamentListResponse[]>>(`/api/v1/filaments/GetFilamentList`);
    return response.data;
}

export const postFilament = async (data: FilamentRequest): Promise<CommonResponse<number>> => {
    const formData = new FormData();

    formData.append("filamentName", data.filamentName);
    formData.append("filamentTemperature", data.filamentTemperature.toString());
    formData.append("filamentColor", data.filamentColor);
    formData.append("filamentCost", data.filamentCost.toString());
    formData.append("filamentLenght", data.filamentLenght.toString());
    formData.append("filamentThickness", data.filamentThickness.toString());
    formData.append("filamentType", data.filamentType.toString());
    formData.append("filamentWeight", data.filamentWeight.toString());
    formData.append("filamentDescription", data.filamentDescription);
    formData.append("groupId", data.groupId.toString());

    if (data.imageFile) {
        formData.append("imageFile", data.imageFile);
    }

    const response = await apiClient.post<CommonResponse<number>>(`/api/v1/filaments/PostFilament`, formData,
        { headers: { "Content-Type": "multipart/form-data" } })
    return response.data
}

