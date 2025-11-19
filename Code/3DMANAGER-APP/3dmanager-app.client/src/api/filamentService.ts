import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { FilamentListResponse } from '../models/filament/FilamentListResponse';

export const getFilamentList = async (type: number): Promise<CommonResponse<FilamentListResponse>> => {
    const response = await apiClient.get<CommonResponse<FilamentListResponse>>(`/api/Filament/GetFilamentList/${type}`);
    return response.data;
}