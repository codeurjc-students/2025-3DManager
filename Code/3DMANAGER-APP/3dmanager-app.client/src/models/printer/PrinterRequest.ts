export interface PrinterRequest {
    groupId: number;
    printerName: string;
    printerDescription: string;
    printerModel: string;
    imageFile?: File | null;
}