import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { CatalogResponse } from '../models/catalog/CatalogResponse';
import type { CatalogPrinterResponse } from '../models/catalog/CatalogPrinterResponse';

export const getFilamentType = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/v1/catalogs/filament-types");
    return response.data;
};

export const getFilamentState = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/v1/catalogs/filament-states");
    return response.data;
};

export const getPrinterCatalog = async (): Promise<CommonResponse<CatalogPrinterResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogPrinterResponse[]>>("/v1/catalogs/printers");
    return response.data;
};

export const getFilamentCatalog = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/v1/catalogs/filaments");
    return response.data;
};

export const getPrintState = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/v1/catalogs/print-states");
    return response.data;
};

export const getPrinterState = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/v1/catalogs/printer-states");
    return response.data;
};