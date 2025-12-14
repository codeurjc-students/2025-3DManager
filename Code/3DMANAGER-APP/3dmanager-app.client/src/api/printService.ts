import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';
import type { PrintRequest } from '../models/print/PrintRequest';

export const getPrintList = async (groupId: number): Promise<CommonResponse<PrintListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<PrintListResponse[]>>(`/api/Print/GetPrintList`, { params: { groupId } });
    return response.data;
}

export const postPrint = async (data: PrintRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/Print/PostPrint', data );
    return response.data;
}