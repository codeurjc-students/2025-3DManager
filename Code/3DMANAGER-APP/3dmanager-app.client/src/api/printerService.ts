import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrinterObject } from '../models/printer/PrinterObject'
import type { PrinterRequest } from '../models/printer/PrinterRequest'
import type { PrinterDashboardObject } from '../models/printer/PrinterDashboardObject'

export const getPrinterList = async (): Promise<CommonResponse<PrinterObject[]>> => {
    const response = await apiClient.get<CommonResponse<PrinterObject[]>>('/api/v1/printers/GetPrinterList')
    return response.data
}

export const postPrinter = async (data: PrinterRequest): Promise<CommonResponse<boolean>> => {
    const response = await apiClient.post<CommonResponse<boolean>>('/api/v1/printers/PostPrinter', data)
    return response.data
}

export const getPrinterDahsboardList = async (): Promise<CommonResponse<PrinterDashboardObject[]>> => {
    const response = await apiClient.get<CommonResponse<PrinterDashboardObject[]>>("/api/v1/printers/GetPrinterDashboardList");
    return response.data;
}

