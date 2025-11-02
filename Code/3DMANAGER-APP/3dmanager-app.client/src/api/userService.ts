import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { UserObject } from '../models/user/userObject'
export const postNewUser = async (data: UserObject): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/User/PostNewUser',data)
    return response.data
}