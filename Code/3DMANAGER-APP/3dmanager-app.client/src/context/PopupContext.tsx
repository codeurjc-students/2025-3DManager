import { createContext, useContext } from "react";
import type { PopupData } from "../models/popup/PopupData";

interface PopupContextType {
    showPopup: (data: PopupData) => void;
    closePopup: () => void;
}

export const PopupContext = createContext<PopupContextType | undefined>(undefined);

export const usePopupContext = () => {
    const context = useContext(PopupContext);
    if (!context) {
        throw new Error("usePopupContext must be used within PopupProvider");
    }
    return context;
};