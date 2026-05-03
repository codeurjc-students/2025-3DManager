import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { LoginResponse } from '../models/user/LoginResponse'
import type { UserCreateRequest } from '../models/user/UserCreateRequest'
import type { LoginRequest } from '../models/user/LoginRequest'
import type { UserListResponse } from '../models/user/UserListResponse'
import type { UserUpdateRequest } from '../models/user/UserUpdateRequest'
import type { UserDetailObject } from '../models/user/UserDetailObject'
import { handleApiError } from '../models/base/handleApiError';

export const postNewUser = async (data: UserCreateRequest): Promise<CommonResponse<number>> => {
    const formData = new FormData();

    formData.append("userName", data.userName);
    formData.append("userPassword", data.userPassword);
    formData.append("userEmail", data.userEmail);

    if (data.imageFile) {
        formData.append("imageFile", data.imageFile);
    }
    const response = await apiClient.post<CommonResponse<number>>('/v1/users', formData,
        { headers: { "Content-Type": "multipart/form-data" }});
    return response.data;
}


export const Login = async (data : LoginRequest): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/v1/users/login', data);
    return response.data;
}

export const LoginGuest = async (): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/v1/users/login/guest');
    return response.data;
}

export const getUserList = async (): Promise<CommonResponse<UserListResponse[]>> => {

    try {
        const response = await apiClient.get<CommonResponse<UserListResponse[]>>("/v1/users");
        return response.data;
    } catch (error: unknown) {
        return handleApiError<UserListResponse[]>(
            error,
            "Error al obtener el listado de usuario"
        );
    }
}

export const getUserInvitationList = async (filter?: string): Promise<CommonResponse<UserListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<UserListResponse[]>>(`/v1/users/invitations`,
        { params: { filter } });
    return response.data;
}

export const postUserInvitation = async (userId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/users/invitations/${userId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al enviar invitación de usuario"
        );
    }
}

export const GetUserAuth = async (): Promise<{ userId: number; groupId: number | null; rolId: string | null; groupName: string | null; token: string | null; }> => {
    const response = await apiClient.get("/v1/users/auth");
    return response.data;
};

export const updateUser = async (data: UserUpdateRequest): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/users/${data.userId}`, data);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar usuario"
        );
    }
}

export const getUserDetail = async (userId: number): Promise<CommonResponse<UserDetailObject>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<UserDetailObject>>(`/v1/users/${userId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<UserDetailObject>(
            error,
            "Error al obtener el detalle de usuario"
        );
    }
}

export const updateUserImage = async (userId: number, file: File): Promise<CommonResponse<boolean>> => {
    const formData = new FormData();
    formData.append("imageFile", file);

    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/users/${userId}/image`,
            formData, { headers: { "Content-Type": "multipart/form-data" } });
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualziar la foto de usuario"
        );
    }
};

export const deleteUserImage = async (userId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/users/${userId}/image`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar la foto de un usuario"
        );
    }
};

export const deleteUser = async (userId : number): Promise<CommonResponse<boolean>> => {

    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/users/${userId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar un usuario"
        );
    }
}