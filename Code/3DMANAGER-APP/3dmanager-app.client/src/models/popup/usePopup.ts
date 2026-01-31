import { useState } from "react";
import type { PopupData } from "./popupData";


export const usePopup = () => {
    const [popup, setPopup] = useState<PopupData | null>(null);

    const showPopup = (data: PopupData) => setPopup(data);
    const closePopup = () => setPopup(null);

    return { popup, showPopup, closePopup };
};
