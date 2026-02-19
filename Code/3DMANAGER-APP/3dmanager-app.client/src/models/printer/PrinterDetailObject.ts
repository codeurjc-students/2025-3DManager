
import type { FileResponse } from "../file/fileResponse";
import type { PrinterEstimations } from "./PrinterEstimations";

export interface PrinterDetailObject {
    printerId: number;
    printerName: string;
    printerModel?: string;
    printerDescription?: string;
    printerStateId?: number;
    printerStateName?: string;
    printerImageData?: FileResponse;
    printerCreateDate: Date;
    printerTotalHours: string;
    printerTotalHoursMonth: string;
    printerPrintsTotal: number;
    printerPrintsTotalMonth: number;
    printerEstimations: PrinterEstimations;
}
