import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { LoginResponse } from '../models/user/LoginResponse'
import type { UserCreateRequest } from '../models/user/UserCreateRequest'
import type { LoginRequest } from '../models/user/LoginRequest'
import type { UserListResponse } from '../models/user/UserListResponse'

export const postNewUser = async (data: UserCreateRequest): Promise<CommonResponse<number>> => {
    const formData = new FormData();

    formData.append("userName", data.userName);
    formData.append("userPassword", data.userPassword);
    formData.append("userEmail", data.userEmail);

    if (data.imageFile) {
        formData.append("imageFile", data.imageFile);
    }
    const response = await apiClient.post<CommonResponse<number>>('/api/v1/users/PostNewUser', formData,
        { headers: { "Content-Type": "multipart/form-data" }});
    return response.data;
}


export const Login = async (data : LoginRequest): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/api/v1/users/Login', data);
    return response.data;
}

export const LoginGuest = async (): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/api/v1/users/LoginGuest');
    return response.data;
}

export const getUserList = async (): Promise<CommonResponse<UserListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<UserListResponse[]>>("/api/v1/users/GetUserList");
    return response.data;
}

export const getUserInvitationList = async (filter?: string): Promise<CommonResponse<UserListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<UserListResponse[]>>(`/api/v1/users/GetUserInvitationList`,
        { params: { filter } });
    return response.data;
}

export const postUserInvitation = async (userId: number): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>(`/api/v1/users/PostUserInvitation?userId=${userId}`);
    return response.data;
}

export const GetUserAuth = async (): Promise<{userId: number; groupId: number | null; rolId: string | null;}> => {
    const response = await apiClient.get("/api/v1/users/GetUserAuth");
    return response.data;
};
