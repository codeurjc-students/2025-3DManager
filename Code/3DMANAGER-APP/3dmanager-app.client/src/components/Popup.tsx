import React from "react";
import type { PopupData } from "../models/popup/popupData";


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
            default:
                return "";
        }
    };

    return (
        <div className="popup-overlay">
            <div className={`popup-container ${getColorClass()}`}>
                <h3 className="popup-title">{data.title}</h3>
                <p className="popup-description">{data.description}</p>

                <button className="popup-button" onClick={onClose}>
                    Aceptar
                </button>
            </div>
        </div>
    );
};

export default Popup;
