import React from "react";

interface ConfirmPopup {
    action: string;
    onConfirm: () => void;
}

const ConfirmPopup: React.FC<ConfirmPopup> = ({action, onConfirm}) => {
    return (
        <div className="confirm-popup">
            <h3 className="popup-title">Confirmar de acción: {action}</h3>
            <p className="popup-description">La acción que vas a realizar no tiene vuelta atrás. ¿Estas seguro de que quieres llevarla a cabo?</p>
            <div>
                <button className="button-yellow w-50 mt-2 " onClick={onConfirm}>
                    Confirmar
                </button>
            </div>
        </div>
    );
};

export default ConfirmPopup;
