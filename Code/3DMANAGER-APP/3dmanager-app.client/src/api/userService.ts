import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { LoginResponse } from '../models/user/LoginResponse'
import type { UserCreateRequest } from '../models/user/UserCreateRequest'
import type { LoginRequest } from '../models/user/LoginRequest'
import type { UserListResponse } from '../models/user/UserListResponse'
import type { UserUpdateRequest } from '../models/user/UserUpdateRequest'
import type { UserDetailObject } from '../models/user/UserDetailObject'

export const postNewUser = async (data: UserCreateRequest): Promise<CommonResponse<number>> => {
    const formData = new FormData();

    formData.append("userName", data.userName);
    formData.append("userPassword", data.userPassword);
    formData.append("userEmail", data.userEmail);

    if (data.imageFile) {
        formData.append("imageFile", data.imageFile);
    }
    const response = await apiClient.post<CommonResponse<number>>('/v1/users/PostNewUser', formData,
        { headers: { "Content-Type": "multipart/form-data" }});
    return response.data;
}


export const Login = async (data : LoginRequest): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/v1/users/Login', data);
    return response.data;
}

export const LoginGuest = async (): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/v1/users/LoginGuest');
    return response.data;
}

export const getUserList = async (): Promise<CommonResponse<UserListResponse[]>> => {

    try {
        const response = await apiClient.get<CommonResponse<UserListResponse[]>>("/v1/users/GetUserList");
        return response.data;
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener el listado de usuarios"
            }
        };
    }
}

export const getUserInvitationList = async (filter?: string): Promise<CommonResponse<UserListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<UserListResponse[]>>(`/v1/users/GetUserInvitationList`,
        { params: { filter } });
    return response.data;
}

export const postUserInvitation = async (userId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/users/PostUserInvitation?userId=${userId}`);
        return response.data;
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al enviar una invitacion al grupo"
            }
        };
    }
}

export const GetUserAuth = async (): Promise<{ userId: number; groupId: number | null; rolId: string | null; groupName: string | null; }> => {
    const response = await apiClient.get("/v1/users/GetUserAuth");
    return response.data;
};

export const updateUser = async (data: UserUpdateRequest): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/users/UpdateUser`, data);
        return response.data;
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al actualizar el usuario"
            }
        };
    }
}

export const getUserDetail = async (userId: number): Promise<CommonResponse<UserDetailObject>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<UserDetailObject>>(`/v1/users/GetUserDetail?userId=${userId}`);
        return response.data;
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener el detalle de usuario"
            }
        };
    }
}

export const updateUserImage = async (userId: number, file: File): Promise<CommonResponse<boolean>> => {
    const formData = new FormData();
    formData.append("imageFile", file);

    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/users/UpdateUserImage?userId=${userId}`,
            formData, { headers: { "Content-Type": "multipart/form-data" } });
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;

        if (backendResponse?.error) return backendResponse;

        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error al actualizar la imagen del usuario"
            }
        };
    }
};

export const deleteUserImage = async (userId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/users/DeleteUserImage?userId=${userId}`);
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;

        if (backendResponse?.error) return backendResponse;
        return {
            data: false,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error al eliminar la imagen del usuario"
            }
        };
    }
};