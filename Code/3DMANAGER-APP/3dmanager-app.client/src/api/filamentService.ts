import apiClient from './apiClient'
import { handleApiError } from '../models/base/handleApiError';
import type { CommonResponse } from '../models/base/CommonResponse'
import type { FilamentListResponse } from '../models/filament/FilamentListResponse';
import type { FilamentRequest } from '../models/filament/FilamentRequest';
import type { FilamentUpdateRequest } from '../models/filament/FilamentUpdateRequest';
import type { FilamentDetailObject } from '../models/filament/FilamentDetailObject';

export const getFilamentList = async (): Promise<CommonResponse<FilamentListResponse[]>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<FilamentListResponse[]>>(`/v1/filaments`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<FilamentListResponse[]>(
            error,
            "Error al recoger el listado de filamentos"
        );
    }
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

    try {
        const response = await apiClient.post<CommonResponse<number>>(`/v1/filaments`, formData,
            { headers: { "Content-Type": "multipart/form-data" } })
        return response.data
    } catch (error: unknown) {
        return handleApiError<number>(
            error,
            "Error al crear filamento"
        );
    }
}

export const updateFilament = async (data: FilamentUpdateRequest): Promise<CommonResponse<boolean>> => {
    
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/filaments/${data.filamentId}`, data);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar el filamento"
        );
    }
}

export const getFilamentDetail = async (filamentId: number): Promise<CommonResponse<FilamentDetailObject>> => {
    try {
        const response = await apiClient.get<CommonResponse<FilamentDetailObject>>(`/v1/filaments/${filamentId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<FilamentDetailObject>(
            error,
            "Error al obtener el detalle de filamento"
        );
    }
}

export const deleteFilament = async (filamentId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/filaments/${filamentId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar un filamento"
        );
    }
}

export const updateFilamentImage = async (filamentId: number, file: File): Promise<CommonResponse<boolean>> => {
    const formData = new FormData();
    formData.append("imageFile", file);

    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/filaments/${filamentId}/image`,
            formData, { headers: { "Content-Type": "multipart/form-data" } });
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar la imagen de filamento"
        );
    }
};

export const deleteFilamentImage = async (filamentId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/filaments/${filamentId}/image`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar la imagen de filamento"
        );
    }
};