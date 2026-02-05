export type PopupType = "info" | "warning" | "error" | "base";

export interface PopupData {
    type?: PopupType;
    content: React.ReactNode; 
    width?: string; 
    height?: string;
}
