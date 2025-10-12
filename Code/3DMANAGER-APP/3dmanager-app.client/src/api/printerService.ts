import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { PrinterObject } from '../models/printer/PrinterObject'

export const getPrinterList = async (): Promise<CommonResponse<PrinterObject[]>> => {
    const response = await apiClient.get<CommonResponse<PrinterObject[]>>('/api/Printer/GetPrinterList')
    return response.data
}