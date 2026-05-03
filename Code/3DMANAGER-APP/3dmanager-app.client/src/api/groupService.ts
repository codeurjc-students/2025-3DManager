import apiClient from './apiClient'
import { handleApiError } from '../models/base/handleApiError';
import type { CommonResponse } from '../models/base/CommonResponse'
import type { GroupRequest } from '../models/group/GroupRequest'
import type { GroupInvitation } from '../models/group/GroupInvitation'
import type { GroupBasicDataResponse } from '../models/group/GroupBasicDataResponse'
import type { GroupDashboardData } from '../models/group/GroupDashboardData'
export const postNewGroup = async (data: GroupRequest): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.post<CommonResponse<boolean>>('/v1/groups', data)
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al crear un grupo"
        );
    }
}

export const getGroupInvitations = async (): Promise<CommonResponse<GroupInvitation[]>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<GroupInvitation[]>>(`/v1/groups/invitations`)
        return response.data
    } catch (error: unknown) {
        return handleApiError<GroupInvitation[]>(
            error,
            "Error al recibir las invitaciones de grupo"
        );
    }
}

export const postAcceptInvitation = async (groupId: number, isAccepted: boolean): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/groups/invitations/${groupId}`, { isAccepted })
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al aceptar invitación de grupo"
        );
    }
    
}

export const getGroupBasicData = async (): Promise<CommonResponse<GroupBasicDataResponse>> => {
    try {
        const response = await apiClient.get<CommonResponse<GroupBasicDataResponse>>('/v1/groups/me')
        return response.data
    } catch (error: unknown) {
        return handleApiError<GroupBasicDataResponse>(
            error,
            "Error al obtener los datos basicos del grupo"
        );
    }
}

export const updateGroupData = async (groupId: number, data: GroupRequest): Promise<CommonResponse<boolean>> => {
    
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}`, data)
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar el grupo"
        );
    }
}
export const leaveGroup = async (groupId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}/leave`)
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al dejar el grupo"
        );
    }
}
export const deleteGroup = async (groupId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/groups/${groupId}`)
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar el grupo"
        );
    }
}
export const kickUserFromGroup = async (groupId: number, userId : number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}/kick/${userId}`)
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al expulsar a un usuario del grupo"
        );
    }
}
export const transferOwnership = async (groupId: number,newOwnerUserId: number): Promise<CommonResponse<boolean>> => {
    
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}/owner`, { newOwnerUserId });
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error trasnferir el rol de dueño del grupo"
        );
    }
}
export const getGroupDashboardData = async (groupId: number): Promise<CommonResponse<GroupDashboardData>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<GroupDashboardData>>(`/v1/groups/${groupId}/dashboard`)
        return response.data
    } catch (error: unknown) {
        return handleApiError<GroupDashboardData>(
            error,
            "Error al obtener la información del panel principal del grupo"
        );
    }
}

