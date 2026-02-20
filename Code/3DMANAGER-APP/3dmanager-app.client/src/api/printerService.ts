import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrinterObject } from '../models/printer/PrinterObject'
import type { PrinterRequest } from '../models/printer/PrinterRequest'
import type { PrinterDashboardObject } from '../models/printer/PrinterDashboardObject'
import type { PrinterDetailObject } from '../models/printer/PrinterDetailObject'
import type { PrinterDetailRequest } from '../models/printer/PrinterDetailRequest'

export const getPrinterList = async (): Promise<CommonResponse<PrinterObject[]>> => {
    const response = await apiClient.get<CommonResponse<PrinterObject[]>>('/api/v1/printers/GetPrinterList')
    return response.data
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
    const response = await apiClient.post<CommonResponse<boolean>>('/api/v1/printers/PostPrinter', formData,
        { headers: { "Content-Type": "multipart/form-data" } })
    return response.data
}

export const getPrinterDashboardList = async (): Promise<CommonResponse<PrinterDashboardObject[]>> => {
    const response = await apiClient.get<CommonResponse<PrinterDashboardObject[]>>("/api/v1/printers/GetPrinterDashboardList");
    return response.data;
}

export const updatePrinter = async (data: PrinterDetailRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.put<CommonResponse<boolean>>(`/api/v1/printers/UpdatePrinter`, data);
    return response.data;
}

export const getPrinterDetail = async (printerId: number): Promise<CommonResponse<PrinterDetailObject>> => {
    const response = await apiClient.get<CommonResponse<PrinterDetailObject>>(`/api/v1/printers/GetPrinterDetail?printerId=${printerId}`);
    return response.data;
}

