import { PopupContext } from "./PopupContext";
import { usePopup } from "../models/popup/usePopup";
import Popup from "../components/Popup";

export const PopupProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const { popup, showPopup, closePopup } = usePopup();

    return (
        <PopupContext.Provider value={{ showPopup, closePopup }}>
            {children}
            <Popup data={popup} onClose={closePopup} />
        </PopupContext.Provider>
    );
};