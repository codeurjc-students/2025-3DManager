import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { GroupRequest } from '../models/group/GroupRequest'
import type { GroupInvitation } from '../models/group/GroupInvitation'

export const postNewGroup = async (data: GroupRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/v1/groups/PostNewGroup', data)
    return response.data
}

export const getGroupInvitations = async (): Promise<CommonResponse<GroupInvitation[]>> => {
    const response = await apiClient.post<CommonResponse<GroupInvitation[]>>('/api/v1/groups/GetGroupInvitations')
    return response.data
}

export const postAcceptInvitation = async (groupId : number): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>(`/api/v1/groups/postAcceptInvitation?groupId=${groupId}`)
    return response.data
}


