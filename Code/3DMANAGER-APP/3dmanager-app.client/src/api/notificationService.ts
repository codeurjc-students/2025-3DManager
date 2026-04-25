import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { NotificationObject } from '../models/notifications/NotificationObject';

export const getUnreadNotifications = async ():
    Promise<CommonResponse<NotificationObject[]>> => {
    try {
        const response = await apiClient.get<CommonResponse<NotificationObject[]>>('/v1/notifications/unread');
        return response.data;
    } catch (error: any) {
        const status = error?.response?.status;
        const backendResponse = error?.response?.data;

        if (backendResponse?.error) {
            return backendResponse;
        }
        return {
            data: [],
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? "Error desconocido en el servidor"
            }
        };
    }
};

export const markNotificationAsRead = async (id: number):
    Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/notifications/${id}/read`);
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
                message: backendResponse?.message ?? "Error desconocido en el servidor"
            }
        };
    }
};




