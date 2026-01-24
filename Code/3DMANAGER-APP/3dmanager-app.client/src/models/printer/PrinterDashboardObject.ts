import type { FileResponse } from "../file/fileResponse";

export interface PrinterDashboardObject {
    printerId?: number;
    printerName?: string;
    printerModel?: string;
    printerDescription?: string;
    printerStateId?: number;
    printerStateName?: string;
    printerImageData?: FileResponse;
}