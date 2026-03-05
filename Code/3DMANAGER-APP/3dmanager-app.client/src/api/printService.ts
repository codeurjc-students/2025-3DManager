import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';
import type { PrintRequest } from '../models/print/PrintRequest';
import type { PrintDetailRequest } from '../models/print/PrintDetailRequest';
import type { PrintDetailObject } from '../models/print/PrintDetailObject';
import type { PrintCommentRequest } from '../models/print/PrintCommentRequest';
import type { PrintCommentObject } from '../models/print/PrintCommentObject';

export const getPrintList = async (pageNumber: number, pageSize: number): Promise<CommonResponse<PrintListResponse>> => {
    const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/api/v1/prints/GetPrintList?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    return response.data;
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
    formData.append("printTime", Math.round(data.printTime).toString());
    if (data.imageFile) {
        formData.append("imageFile", data.imageFile);
    }
    const response = await apiClient.post<CommonResponse<number>>(`/api/v1/prints/PostPrint`, formData,
        { headers: { "Content-Type": "multipart/form-data" } })
    return response.data;
}

export const GetPrintListByType = async (pageNumber: number, pageSize: number , type : number ,id : number): Promise<CommonResponse<PrintListResponse>> => {
    const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/api/v1/prints/GetPrintListByType?pageNumber=${pageNumber}&pageSize=${pageSize}&type=${type}&id=${id}`);
    return response.data;
}

export const updatePrint = async (data: PrintDetailRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.put<CommonResponse<boolean>>(`/api/v1/prints/UpdatePrint`, data);
    return response.data;
}

export const getPrintDetail = async (printId: number): Promise<CommonResponse<PrintDetailObject>> => {
    const response = await apiClient.get<CommonResponse<PrintDetailObject>>(`/api/v1/prints/GetPrintDetail?printId=${printId}`);
    return response.data;
}

export const getPrintComments = async (printId: number): Promise<CommonResponse<PrintCommentObject[]>> => {
    const response = await apiClient.get(`/api/v1/prints/GetPrintComments?printId=${printId}`);
    return response.data;
};

export const postPrintComment = async (data: PrintCommentRequest): Promise<CommonResponse<number>> => {
    const response = await apiClient.post(`/api/v1/prints/PostPrintComment`, data);
    return response.data;
};


