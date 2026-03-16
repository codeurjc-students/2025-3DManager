import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrinterObject } from '../models/printer/PrinterObject'
import type { PrinterRequest } from '../models/printer/PrinterRequest'
import type { PrinterDashboardObject } from '../models/printer/PrinterDashboardObject'
import type { PrinterDetailObject } from '../models/printer/PrinterDetailObject'
import type { PrinterDetailRequest } from '../models/printer/PrinterDetailRequest'

export const getPrinterList = async (): Promise<CommonResponse<PrinterObject[]>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<PrinterObject[]>>('/api/v1/printers/GetPrinterList')
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener el listado de impresoras (Deprecated)"
            }
        };
    }
}

export const postPrinter = async (data: PrinterRequest): Promise<CommonResponse<boolean>> => {
    const formData = new FormData();

    formData.append("printerName", data.printerName);
    formData.append("printerDescription", data.printerDescription);
    formData.append("printerModel", data.printerModel);
    formData.append("groupId", data.groupId.toString());

    if (data.imageFile) {
        formData.append("imageFile", data.imageFile);
    }
    
    try {
        const response = await apiClient.post<CommonResponse<boolean>>('/api/v1/printers/PostPrinter', formData,
            { headers: { "Content-Type": "multipart/form-data" } })
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al crear una impresora"
            }
        };
    }
}

export const getPrinterDashboardList = async (): Promise<CommonResponse<PrinterDashboardObject[]>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrinterDashboardObject[]>>("/api/v1/printers/GetPrinterDashboardList");
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al traer el listado de impresorar del dashboard"
            }
        };
    }
}

export const updatePrinter = async (data: PrinterDetailRequest): Promise<CommonResponse<boolean>> => {  
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/api/v1/printers/UpdatePrinter`, data);
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al actualizar la impresora"
            }
        };
    }
}

export const getPrinterDetail = async (printerId: number): Promise<CommonResponse<PrinterDetailObject>> => { 
    try {
        const response = await apiClient.get<CommonResponse<PrinterDetailObject>>(`/api/v1/printers/GetPrinterDetail?printerId=${printerId}`);
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al obtener el detalle de impresora"
            }
        };
    }
}

export const deletePrinter = async (printerId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/api/v1/printers/DeletePrinter?printerId=${printerId}`);
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
                message: backendResponse?.message ?? "Error desconocido en el servidor al tratar de eliminar una impresora"
            }
        };
    }
}
