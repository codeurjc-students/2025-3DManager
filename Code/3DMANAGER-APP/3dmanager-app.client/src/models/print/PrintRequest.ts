export interface PrintRequest {
    groupId: number;
    userId: number;
    printName: string;
    printState: number;
    printPrinter: number;
    printFilament: number;
    printDescription: string;
    printTime: number;
    printRealTime: number;
    printFilamentUsed: number;
    imageFile?: File | null;
}