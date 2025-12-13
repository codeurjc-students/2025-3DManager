import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { LoginResponse } from '../models/user/LoginResponse'
import type { UserCreateRequest } from '../models/user/UserCreateRequest'
import type { LoginRequest } from '../models/user/LoginRequest'
import type { UserListResponse } from '../models/user/UserListResponse'

export const postNewUser = async (data: UserCreateRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/User/PostNewUser', data);
    return response.data;
}

export const Login = async (data : LoginRequest): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/api/User/Login', data);
    return response.data;
}

export const LoginGuest = async (): Promise<CommonResponse<LoginResponse>> => {
    const response = await apiClient.post<CommonResponse<LoginResponse>>('/api/User/LoginGuest');
    return response.data;
}

export const getUserList = async (groupId: number): Promise<CommonResponse<UserListResponse[]>> => {
    const response = await apiClient.get<CommonResponse<UserListResponse[]>>(`/api/User/GetUserList`, { params: { groupId } });
    return response.data;
}