import type { PrintResponse } from "./PrintResponse";

export interface PrintListResponse {
    prints: PrintResponse[];
    totalItems: number;
    totalPages: number;
}