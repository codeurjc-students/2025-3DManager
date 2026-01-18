import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { FileResponse } from '../models/file/fileResponse';

export const getUploadUrl = async (file: File): Promise<CommonResponse<FileResponse>> => {
    const response = await apiClient.post<CommonResponse<FileResponse>>('/api/v1/files/GetUploadUrl', { fileName: file.name, contentType: file.type, });
    return response.data;
}
