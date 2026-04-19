import React from "react";
import { useNavigate } from "react-router-dom";

interface Props {
    onClose: () => void;
}

const InsertInventoryPopup: React.FC<Props> = ({ onClose }) => {
    const navigate = useNavigate();

    const go = (path: string) => {
        onClose(); 
        navigate(path);
    };

    return (
        <div className="insert-inventory-popup">
            <h2 className="popup-title mb-4">Agregar al inventario</h2>

            <div className="popup-buttons-container">

                <button
                    type="button"
                    className="button-low-yellow popup-option"
                    onClick={() => go("/dashboard/printer-create")}
                >
                    <span className="dashboard-title mt-2">Agregar impresora</span>
                </button>

                <button
                    type="button"
                    className="button-low-yellow popup-option"
                    onClick={() => go("/dashboard/filament-create")}
                >
                    <span className="dashboard-title mt-2">Agregar filamento</span>
                </button>

                <button
                    type="button"
                    className="button-low-yellow popup-option"
                    onClick={() => go("/dashboard/user-invitation/invitations")}
                >
                    <span className="dashboard-title mt-2">Agregar usuario al grupo</span>
                </button>

            </div>
        </div>
    );
};

export default InsertInventoryPopup;
