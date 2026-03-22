import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { CatalogResponse } from '../models/catalog/CatalogResponse';
import type { CatalogPrinterResponse } from '../models/catalog/CatalogPrinterResponse';

export const getFilamentType = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/v1/catalogs/GetFilamentType");
    return response.data;
};

export const getFilamentState = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/v1/catalogs/GetFilamentState");
    return response.data;
};

export const getPrinterCatalog = async (): Promise<CommonResponse<CatalogPrinterResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogPrinterResponse[]>>("/api/v1/catalogs/GetPrinterCatalog");
    return response.data;
};

export const getFilamentCatalog = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/v1/catalogs/GetFilamentCatalog");
    return response.data;
};

export const getPrintState = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/v1/catalogs/GetPrintState");
    return response.data;
};

export const getPrinterState = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/v1/catalogs/GetPrinterState");
    return response.data;
};