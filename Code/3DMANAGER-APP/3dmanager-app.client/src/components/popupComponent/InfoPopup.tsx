// src/components/popupContents/BasicPopupContent.tsx
import React from "react";

interface InfoPopup {
    title: string;
    description: string;
}

const InfoPopup: React.FC<InfoPopup> = ({ title, description }) => {
    return (
        <div className="info-popup-content">
            <h3 className="popup-title">{title}</h3>
            <p className="popup-description">{description}</p>
        </div>
    );
};

export default InfoPopup;
