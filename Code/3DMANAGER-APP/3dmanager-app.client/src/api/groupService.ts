import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { GroupRequest } from '../models/group/GroupRequest'

export const postNewGroup = async (data: GroupRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/v1/groups/PostNewGroup', data)
    return response.data
}

