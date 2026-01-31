export type PopupType = "info" | "warning" | "error";

export interface PopupData {
    type: PopupType;
    title: string;
    description: string;
}
