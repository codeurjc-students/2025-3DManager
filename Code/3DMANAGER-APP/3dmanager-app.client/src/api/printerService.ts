import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrinterObject } from '../models/printer/PrinterObject'
import type { PrinterRequest } from '../models/printer/PrinterRequest'
import type { PrinterDashboardObject } from '../models/printer/PrinterDashboardObject'

export const getPrinterList = async (): Promise<CommonResponse<PrinterObject[]>> => {
    const response = await apiClient.get<CommonResponse<PrinterObject[]>>('/api/Printer/GetPrinterList')
    return response.data
}

export const postPrinter = async (data: PrinterRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/Printer/PostPrinter', data)
    return response.data
}

export const getPrinterDahsboardList = async (groupId: number): Promise<CommonResponse<PrinterDashboardObject[]>> => {
    const response = await apiClient.get<CommonResponse<PrinterDashboardObject[]>>(`/api/Printer/GetPrinterDashboardList`, { params: { groupId } });
    return response.data;
}

