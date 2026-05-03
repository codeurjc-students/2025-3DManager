import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { NotificationObject } from '../models/notifications/NotificationObject';
import { handleApiError } from '../models/base/handleApiError';

export const getUnreadNotifications = async ():
    Promise<CommonResponse<NotificationObject[]>> => {
    try {
        const response = await apiClient.get<CommonResponse<NotificationObject[]>>('/v1/notifications/unread');
        return response.data;
    } catch (error: unknown) {
        return handleApiError<NotificationObject[]>(
            error,
            "Error al obtener el listado de notificaciones sin leer"
        );
    }
};

export const markNotificationAsRead = async (id: number):
    Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/notifications/${id}/read`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al marcar una notificación como leída"
        );
    }
};




