import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';
import type { PrintRequest } from '../models/print/PrintRequest';
import type { PrintDetailRequest } from '../models/print/PrintDetailRequest';
import type { PrintDetailObject } from '../models/print/PrintDetailObject';
import type { PrintCommentRequest } from '../models/print/PrintCommentRequest';
import type { PrintCommentObject } from '../models/print/PrintCommentObject';
import { handleApiError } from '../models/base/handleApiError';

export const getPrintList = async (pageNumber: number, pageSize: number): Promise<CommonResponse<PrintListResponse>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/v1/prints?pageNumber=${pageNumber}&pageSize=${pageSize}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<PrintListResponse>(
            error,
            "Error al obtener listado de impresiones"
        );
    }
}

export const postPrint = async (data: PrintRequest): Promise<CommonResponse<number>> => {
    const formData = new FormData();

    formData.append("groupId", data.groupId.toString());
    formData.append("userId", data.userId.toString());
    formData.append("printDescription", data.printDescription);
    formData.append("printFilament", data.printFilament.toString());
    formData.append("printFilamentUsed", data.printFilamentUsed.toString());
    formData.append("printName", data.printName);
    formData.append("printPrinter", data.printPrinter.toString());
    formData.append("printRealTime", data.printRealTime.toString());
    formData.append("printState", data.printState.toString());
    formData.append("printProgress", data.printProgress.toString());
    formData.append("printTime", Math.round(data.printTime).toString());
    if (data.imageFile) {
        formData.append("imageFile", data.imageFile);
    }
    
    try {
        const response = await apiClient.post<CommonResponse<number>>(`/v1/prints`, formData,
            { headers: { "Content-Type": "multipart/form-data" } })
        return response.data;
    } catch (error: unknown) {
        return handleApiError<number>(
            error,
            "Error al crear impresión"
        );
    }
}

export const GetPrintListByType = async (pageNumber: number, pageSize: number , type : number ,id : number): Promise<CommonResponse<PrintListResponse>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/v1/prints/type/${type}/${id}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<PrintListResponse>(
            error,
            "Error al tarer el listado de impresiones"
        );
    }
}

export const updatePrint = async (data: PrintDetailRequest): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/prints/${data.printId}`, data);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar la impresión"
        );
    }
}

export const getPrintDetail = async (printId: number): Promise<CommonResponse<PrintDetailObject>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrintDetailObject>>(`/v1/prints/${printId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<PrintDetailObject>(
            error,
            "Error al obtener el detalle de impresión"
        );
    }
}

export const getPrintComments = async (printId: number): Promise<CommonResponse<PrintCommentObject[]>> => {
    try {
        const response = await apiClient.get(`/v1/prints/${printId}/comments`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<PrintCommentObject[]>(
            error,
            "Error al obtener los comentarios sobre la impresión"
        );
    }
};

export const postPrintComment = async (data: PrintCommentRequest): Promise<CommonResponse<number>> => {
    try {
        const response = await apiClient.post(`/v1/prints/${data.printId}/comments`, data);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<number>(
            error,
            "Error al comentar en la impresión"
        );
    }
};

export const deletePrint = async (printId: number): Promise<CommonResponse<boolean>> => {
    
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/prints/${printId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar una impresión"
        );
    }
}

export const updatePrintImage = async (printId: number, file: File): Promise<CommonResponse<boolean>> => {
    const formData = new FormData();
    formData.append("imageFile", file);

    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/prints/${printId}/image`,
            formData, { headers: { "Content-Type": "multipart/form-data" } });
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar el fichero STL de la impresión"
        );
    }
};

export const deletePrintImage = async (printId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/prints/${printId}/image`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar la imagen de impresión"
        );
    }
};

export const deletePrintComment = async (commentId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete(`/v1/prints/comments/${commentId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar un comentario"
        );
    }
};


