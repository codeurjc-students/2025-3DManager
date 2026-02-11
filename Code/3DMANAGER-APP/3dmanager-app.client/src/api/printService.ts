import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';
import type { PrintRequest } from '../models/print/PrintRequest';

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
