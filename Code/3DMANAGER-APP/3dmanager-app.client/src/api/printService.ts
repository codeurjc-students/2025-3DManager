import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrintListResponse } from '../models/print/PrintListResponse';

export const getPrintList = async (type: number): Promise<CommonResponse<PrintListResponse>> => {
    const response = await apiClient.get<CommonResponse<PrintListResponse>>(`/api/Print/GetPrintList/${type}`);
    return response.data;
}