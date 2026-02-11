import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { GroupRequest } from '../models/group/GroupRequest'
import type { GroupInvitation } from '../models/group/GroupInvitation'
import type { GroupBasicDataResponse } from '../models/group/GroupBasicDataResponse'
export const postNewGroup = async (data: GroupRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/v1/groups/PostNewGroup', data)
    return response.data
}

export const getGroupInvitations = async (): Promise<CommonResponse<GroupInvitation[]>> => {
    const response = await apiClient.post<CommonResponse<GroupInvitation[]>>('/api/v1/groups/GetGroupInvitations')
    return response.data
}

export const postAcceptInvitation = async (groupId: number, isAccepted: boolean): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>(`/api/v1/groups/postAcceptInvitation?groupId=${groupId}&isAccepted=${isAccepted}`)
    return response.data
}

export const getGroupBasicData = async (): Promise<CommonResponse<GroupBasicDataResponse>> => {
    const response = await apiClient.get<CommonResponse<GroupBasicDataResponse>>('/api/v1/groups/GetGroupBasicData')
    return response.data
}

export const updateGroupData = async (data: GroupRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.put<CommonResponse<boolean>>('/api/v1/groups/UpdateGroupData', data)
    return response.data
}
export const leaveGroup = async (): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.put<CommonResponse<boolean>>('/api/v1/groups/UpdateLeaveGroup')
    return response.data
}
export const deleteGroup = async (): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.delete<CommonResponse<boolean>>('/api/v1/groups/DeleteGroup')
    return response.data
}
export const kickUserFromGroup = async (userId : number): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.put<CommonResponse<boolean>>(`/api/v1/groups/UpdateMembership?userKickedId=${userId}`)
    return response.data
}
export const transferOwnership = async (userId: number): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.put<CommonResponse<boolean>>(`/api/v1/groups/TrasnferOwnership?newOwnerUserId=${userId}`)
    return response.data
}
