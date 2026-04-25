import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { GroupRequest } from '../models/group/GroupRequest'
import type { GroupInvitation } from '../models/group/GroupInvitation'
import type { GroupBasicDataResponse } from '../models/group/GroupBasicDataResponse'
import type { GroupDashboardData } from '../models/group/GroupDashboardData'
export const postNewGroup = async (data: GroupRequest): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.post<CommonResponse<boolean>>('/v1/groups', data)
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor"
            }
        };
    }
}

export const getGroupInvitations = async (): Promise<CommonResponse<GroupInvitation[]>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<GroupInvitation[]>>(`/v1/groups/invitations`)
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor"
            }
        };
    }
}

export const postAcceptInvitation = async (groupId: number, isAccepted: boolean): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/groups/invitations/${groupId}`, { isAccepted })
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al tratar de aceptar una invitación al grupo"
            }
        };
    }
}

export const getGroupBasicData = async (): Promise<CommonResponse<GroupBasicDataResponse>> => {
    try {
        const response = await apiClient.get<CommonResponse<GroupBasicDataResponse>>('/v1/groups/me')
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener los datos basicos de grupo"
            }
        };
    }
}

export const updateGroupData = async (groupId: number, data: GroupRequest): Promise<CommonResponse<boolean>> => {
    
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}`, data)
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al actualizar los datos del grupo"
            }
        };
    }
}
export const leaveGroup = async (groupId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}/leave`)
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al tratar de abandonar el grupo"
            }
        };
    }
}
export const deleteGroup = async (groupId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/groups/${groupId}`)
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al tratar de eliminar el grupo"
            }
        };
    }
}
export const kickUserFromGroup = async (groupId: number, userId : number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}/kick/${userId}`)
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al expulsar a un usario del grupo"
            }
        };
    }
}
export const transferOwnership = async (groupId: number,newOwnerUserId: number): Promise<CommonResponse<boolean>> => {
    
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/groups/${groupId}/owner`, { newOwnerUserId });
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al tranferir el rol de manager de grupo"
            }
        };
    }
}
export const getGroupDashboardData = async (groupId: number): Promise<CommonResponse<GroupDashboardData>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<GroupDashboardData>>(`/v1/groups/${groupId}/dashboard`)
        return response.data
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;
        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener la información del dashboard de grupo"
            }
        };
    }
}

