import apiClient from './apiClient'
import type { CommonResponse } from '../models/base/CommonResponse'
import type { CatalogResponse } from '../models/catalog/CatalogResponse';

export const getFilamentType = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/Catalog/GetFilamentType");
    return response.data;
};

export const getPrinterCatalog = async ( groupId : number): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/Catalog/GetPrinterCatalog", { params: { groupId } });
    return response.data;
};

export const getFilamentCatalog = async (groupId: number): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/Catalog/GetFilamentCatalog", { params: { groupId } });
    return response.data;
};

export const getPrintState = async (): Promise<CommonResponse<CatalogResponse[]>> => {
    const response = await apiClient.get<CommonResponse<CatalogResponse[]>>("/api/Catalog/GetPrintState");
    return response.data;
};