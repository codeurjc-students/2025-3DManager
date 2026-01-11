import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { CatalogResponse } from '../models/catalog/CatalogResponse';

export const getFilamentType = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/v1/catalogs/GetFilamentType");
    return response.data;
};

export const getPrinterCatalog = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/v1/catalogs/GetPrinterCatalog");
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