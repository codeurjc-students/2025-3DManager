import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';
import type { PrintRequest } from '../models/print/PrintRequest';

export const getPrintList = async (pageNumber: number, pageSize: number): Promise<CommonResponse<PrintListResponse>> => {
    const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/api/v1/prints/GetPrintList?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    return response.data;
}

export const postPrint = async (data: PrintRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/v1/prints/PostPrint', data );
    return response.data;
}