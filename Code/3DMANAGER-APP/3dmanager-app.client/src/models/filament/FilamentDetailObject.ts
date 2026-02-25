export interface FilamentDetailObject {
    groupId: number;
    filamentId: number;
    filamentName: string;
    filamentType: number;
    filamentWeight: number;
    filamentColor: string;
    filamentTemperature: number;
    filamentLenght: number;
    filamentRemainignLenght: number;
    filamentThickness: number;
    filamentCost: number;
    filamentDescription: string;
    filamentCreateDate: Date;
    filamentState: number;
    filamentPrintedPrintsMonth: number;
    filamentPrintedPrintsTotal: number;
    imageFile?: File | null;
}