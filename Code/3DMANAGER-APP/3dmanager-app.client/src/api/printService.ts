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
        const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/v1/prints?pageNumber=${pageNumber}&pageSize=${pageSize}`);
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
        const response = await apiClient.post<CommonResponse<number>>(`/v1/prints`, formData,
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
        const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/v1/prints/type/${type}/${id}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
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
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/prints/${data.printId}`, data);
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
        const response = await apiClient.get<CommonResponse<PrintDetailObject>>(`/v1/prints/${printId}`);
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
        const response = await apiClient.get(`/v1/prints/${printId}/comments`);
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
        const response = await apiClient.post(`/v1/prints/${data.printId}/comments`, data);
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
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/prints/${printId}`);
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

export const updatePrintImage = async (printId: number, file: File): Promise<CommonResponse<boolean>> => {
    const formData = new FormData();
    formData.append("imageFile", file);

    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/prints/${printId}/image`,
            formData, { headers: { "Content-Type": "multipart/form-data" } });
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;

        if (backendResponse?.error) return backendResponse;

        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error al actualizar el fichero STL de la pieza"
            }
        };
    }
};

export const deletePrintImage = async (printId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/prints/${printId}/image`);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;

        if (backendResponse?.error) return backendResponse;

        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error al eliminar el fichero STL de la impresora"
            }
        };
    }
};

export const deletePrintComment = async (commentId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete(`/v1/prints/comments/${commentId}`);
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al eliminar un comentario sobre una impresion"
            }
        };
    }
};


