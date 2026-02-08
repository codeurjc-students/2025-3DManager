import React from "react";
import type { PopupData } from "../models/popup/PopupData";

interface PopupProps {
    data: PopupData | null;
    onClose: () => void;
}

const Popup: React.FC<PopupProps> = ({ data, onClose }) => {
    if (!data) return null;

    const getColorClass = () => {
        switch (data.type) {
            case "info":
                return "popup-info";
            case "warning":
                return "popup-warning";
            case "error":
                return "popup-error";
            case "base":
                return "popup-base";
            default:
                return "";
        }
    };

    return (
        <div className="popup-overlay">
            <div className={`popup-container ${getColorClass()}`} style={data.width ? { width: data.width } : undefined}>

                <div className="popup-content">
                    {data.content}
                </div>
                <button className="popup-button w-50 mt-2" onClick={onClose}>
                    Cerrar
                </button>
            </div>
        </div>
    );
};

export default Popup;
