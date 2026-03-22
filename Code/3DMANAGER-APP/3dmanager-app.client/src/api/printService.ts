import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';
import type { PrintRequest } from '../models/print/PrintRequest';
import type { PrintDetailRequest } from '../models/print/PrintDetailRequest';
import type { PrintDetailObject } from '../models/print/PrintDetailObject';
import type { PrintCommentRequest } from '../models/print/PrintCommentRequest';
import type { PrintCommentObject } from '../models/print/PrintCommentObject';

export const getPrintList = async (pageNumber: number, pageSize: number): Promise<CommonResponse<PrintListResponse>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/api/v1/prints/GetPrintList?pageNumber=${pageNumber}&pageSize=${pageSize}`);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al traer el listado de impresiones"
            }
        };
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
        const response = await apiClient.post<CommonResponse<number>>(`/api/v1/prints/PostPrint`, formData,
            { headers: { "Content-Type": "multipart/form-data" } })
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al crear una impresion"
            }
        };
    }
}

export const GetPrintListByType = async (pageNumber: number, pageSize: number , type : number ,id : number): Promise<CommonResponse<PrintListResponse>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/api/v1/prints/GetPrintListByType?pageNumber=${pageNumber}&pageSize=${pageSize}&type=${type}&id=${id}`);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al traer el listado de impresiones para el detalle"
            }
        };
    }
}

export const updatePrint = async (data: PrintDetailRequest): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/api/v1/prints/UpdatePrint`, data);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al actualizar la impresión"
            }
        };
    }
}

export const getPrintDetail = async (printId: number): Promise<CommonResponse<PrintDetailObject>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrintDetailObject>>(`/api/v1/prints/GetPrintDetail?printId=${printId}`);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener el detalle de impresión"
            }
        };
    }
}

export const getPrintComments = async (printId: number): Promise<CommonResponse<PrintCommentObject[]>> => {
    try {
        const response = await apiClient.get(`/api/v1/prints/GetPrintComments?printId=${printId}`);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener el listado de comentarios"
            }
        };
    }
};

export const postPrintComment = async (data: PrintCommentRequest): Promise<CommonResponse<number>> => {
    try {
        const response = await apiClient.post(`/api/v1/prints/PostPrintComment`, data);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al añadir un comentario sobre una impresion"
            }
        };
    }
};

export const deletePrint = async (printId: number): Promise<CommonResponse<boolean>> => {
    
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/api/v1/prints/DeletePrint?printId=${printId}`);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al tratar de eliminar una impresión"
            }
        };
    }
}


