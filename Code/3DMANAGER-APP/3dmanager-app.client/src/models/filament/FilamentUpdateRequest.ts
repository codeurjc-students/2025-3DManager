export interface FilamentUpdateRequest {
    groupId: number;
    filamentId: number;
    filamentName: string;
    filamentColor: string;
    filamentTemperature: number;
    filamentLenght: number;
    filamentDescription: string;
    filamentCost: number;
    //imageFile?: File | null;
}