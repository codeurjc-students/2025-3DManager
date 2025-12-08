import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';
import type { PrintRequest } from '../models/print/PrintRequest';

export const getPrintList = async (type: number): Promise<CommonResponse<PrintListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<PrintListResponse[]>>(`/api/Print/GetPrintList`, { params: { type } });
    return response.data;
}

export const postPrint = async (data: PrintRequest): Promise<CommonResponse<PrintListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<PrintListResponse[]>>(`/api/Print/PostPrint`, { params: { data } });
    return response.data;
}