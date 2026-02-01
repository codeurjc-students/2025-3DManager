import React, { createContext, useContext } from "react";
import { usePopup } from "../models/popup/usePopup";
import Popup from "../components/Popup";
import type { PopupData } from "../models/popup/PopupData";

interface PopupContextType {
    showPopup: (data: PopupData) => void;
}

const PopupContext = createContext<PopupContextType | undefined>(undefined);

export const PopupProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const { popup, showPopup, closePopup } = usePopup();

    return (
        <PopupContext.Provider value= {{ showPopup }}>
            {children}
        <Popup data={popup} onClose={closePopup} />
        </PopupContext.Provider>
    );
};

export const usePopupContext = () => {
    const context = useContext(PopupContext);
    if (!context) {
        throw new Error("usePopupContext must be used within PopupProvider");
    }
    return context;
};
