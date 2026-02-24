import type { FileResponse } from "../file/fileResponse";

export interface UserDetailObject {
    userId: number;
    userName: string;
    userRole: string;
    userCreateDate: Date;
    userEmail: string;
    userPrintHours: string;
    userPrintedPrints: number;
    userTotalHours: string;
    userTotalPrints: number;
    userImageData?: FileResponse;
}