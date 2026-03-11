export interface PrinterHoursObject {
    printerId: number;
    printerName: string;
    printerHours: string;
}

export interface GroupDashboardData {
    groupTotalHours: string;
    groupTotalPrints: number;
    groupTotalFilament: number;
    groupUserCount: number;
    groupFilamentCount: number;
    groupPrinterCount: number;
    groupPrinterHours: PrinterHoursObject[];
}
