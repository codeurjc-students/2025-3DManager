import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { FilamentListResponse } from '../models/filament/FilamentListResponse';
import type { FilamentRequest } from '../models/filament/FilamentRequest';
import type { FilamentUpdateRequest } from '../models/filament/FilamentUpdateRequest';
import type { FilamentDetailObject } from '../models/filament/FilamentDetailObject';

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

export const updateFilament = async (data: FilamentUpdateRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.put<CommonResponse<boolean>>(`/api/v1/filaments/UpdateFilament`, data);
    return response.data;
}

export const getFilamentDetail = async (filamentId: number): Promise<CommonResponse<FilamentDetailObject>> => {
    const response = await apiClient.get<CommonResponse<FilamentDetailObject>>(`/api/v1/filaments/GetFilamentDetail?filamentId=${filamentId}`);
    return response.data;
}

export const deleteFilament = async (filamentId: number): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.delete<CommonResponse<boolean>>(`/api/v1/filaments/DeleteFilament?printId=${filamentId}`);
    return response.data;
}


