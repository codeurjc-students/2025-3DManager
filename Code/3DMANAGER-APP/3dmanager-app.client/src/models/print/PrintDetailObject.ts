import type { FileResponse } from "../file/fileResponse";

export interface PrintDetailObject {
    printId: number;
    printFilamentId: number;
    printFilamentName: string;
    printMaterial: string;
    printPrinterId: number;
    printPrinterName: string;
    printUserId: number;
    printUserName: string;
    printName: string;
    printState: number;
    printDescription : string;
    printImageData? : FileResponse | null;
    printCreateDate: Date;
    printMaterialConsumed: number;
    printTimeImpression: string;
    printRealTimeImpression: string;
    printEstimedCost: number;
}