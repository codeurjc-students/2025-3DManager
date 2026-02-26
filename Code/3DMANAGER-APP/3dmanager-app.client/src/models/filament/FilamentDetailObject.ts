import type { FileResponse } from "../file/fileResponse";

export interface FilamentDetailObject {
    groupId: number;
    filamentId: number;
    filamentName: string;
    filamentType: string;
    filamentWeight: number;
    filamentColor: string;
    filamentTemperature: number;
    filamentLenght: number;
    filamentRemainingLenght: number;
    filamentThickness: number;
    filamentCost: number;
    filamentDescription: string;
    filamentCreateDate: Date;
    filamentState: number;
    filamentPrintedPrintsMonth: number;
    filamentPrintedPrintsTotal: number;
    filamentImageFile?: FileResponse | null;
}