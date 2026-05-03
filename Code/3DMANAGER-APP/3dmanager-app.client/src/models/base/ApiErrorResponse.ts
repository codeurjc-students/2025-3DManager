type ApiErrorResponse = {
    error?: {
        code: number;
        message: string;
    };
    message?: string;
};

export type { ApiErrorResponse };