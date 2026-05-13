import axios from "axios";
import type { ApiErrorResponse } from "./ApiErrorResponse";
import type { CommonResponse } from "./CommonResponse";

export const handleApiError = <T>(
    error: unknown,
    defaultMessage: string
): CommonResponse<T> => {

    if (axios.isAxiosError<ApiErrorResponse>(error)) {
        const status = error.response?.status;
        const backendResponse = error.response?.data;

        if (backendResponse?.error) {
            return {
                data: undefined,
                error: backendResponse.error
            };
        }

        return {
            data: undefined,
            error: {
                code: status ?? 500,
                message: backendResponse?.message ?? defaultMessage
            }
        };
    }

    return {
        data: undefined,
        error: {
            code: 500,
            message: defaultMessage
        }
    };
};