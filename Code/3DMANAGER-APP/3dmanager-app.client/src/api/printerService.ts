import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrinterObject } from '../models/printer/PrinterObject'
import type { PrinterRequest } from '../models/printer/PrinterRequest'
import type { PrinterDashboardObject } from '../models/printer/PrinterDashboardObject'
import type { PrinterDetailObject } from '../models/printer/PrinterDetailObject'
import type { PrinterDetailRequest } from '../models/printer/PrinterDetailRequest'
import { handleApiError } from '../models/base/handleApiError';

export const getPrinterList = async (): Promise<CommonResponse<PrinterObject[]>> => {
    
    try {
        const response = await apiClient.get<CommonResponse<PrinterObject[]>>('/v1/printers')
        return response.data
    } catch (error: unknown) {
        return handleApiError<PrinterObject[]>(
            error,
            "Error al obtener las impresoras"
        );
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
        const response = await apiClient.post<CommonResponse<boolean>>('/v1/printers', formData,
            { headers: { "Content-Type": "multipart/form-data" } })
        return response.data
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al crear una impresora"
        );
    }
}

export const getPrinterDashboardList = async (): Promise<CommonResponse<PrinterDashboardObject[]>> => {
    try {
        const response = await apiClient.get<CommonResponse<PrinterDashboardObject[]>>('/v1/printers/dashboard');
        return response.data;
    } catch (error: unknown) {
        return handleApiError<PrinterDashboardObject[]>(
            error,
            "Error al obtener el listado de impresoras"
        );
    }
}

export const updatePrinter = async (data: PrinterDetailRequest): Promise<CommonResponse<boolean>> => {  
    try {
        const response = await apiClient.put<CommonResponse<boolean>>(`/v1/printers/${data.printerId}`, data);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar una impresora"
        );
    }
}

export const getPrinterDetail = async (printerId: number): Promise<CommonResponse<PrinterDetailObject>> => { 
    try {
        const response = await apiClient.get<CommonResponse<PrinterDetailObject>>(`/v1/printers/${printerId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<PrinterDetailObject>(
            error,
            "Error al obtener el detalle de impresora"
        );
    }
}

export const deletePrinter = async (printerId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/printers/${printerId}`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar una impresora"
        );
    }
}

export const updatePrinterImage = async (printerId: number,file: File): Promise<CommonResponse<boolean>> => {
    const formData = new FormData();
    formData.append("imageFile", file);

    try {
        const response = await apiClient.post<CommonResponse<boolean>>(`/v1/printers/${printerId}/image`,
            formData, { headers: { "Content-Type": "multipart/form-data" }});
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al actualizar la imagen de impresora"
        );
    }
};

export const deletePrinterImage = async (printerId: number): Promise<CommonResponse<boolean>> => {
    try {
        const response = await apiClient.delete<CommonResponse<boolean>>(`/v1/printers/${printerId}/image`);
        return response.data;
    } catch (error: unknown) {
        return handleApiError<boolean>(
            error,
            "Error al eliminar la imagen de impresora"
        );
    }
};

