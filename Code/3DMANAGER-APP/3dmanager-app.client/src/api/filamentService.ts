import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { FilamentListResponse } from '../models/filament/FilamentListResponse';
import type { FilamentRequest } from '../models/filament/FilamentRequest';

export const getFilamentList = async (groupId: number): Promise<CommonResponse<FilamentListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<FilamentListResponse[]>>(`/api/Filament/GetFilamentList`, { params: { groupId } });
    return response.data;
}

export const postFilament = async (data: FilamentRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/Filament/PostFilament', data)
    return response.data
}