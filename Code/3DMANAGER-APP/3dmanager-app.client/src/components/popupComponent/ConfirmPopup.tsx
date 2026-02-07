import React from "react";

interface ConfirmPopup {
    onConfirm: () => void;
}

const ConfirmPopup: React.FC<ConfirmPopup> = ({ onConfirm}) => {
    return (
        <div className="confirm-popup">
            <h3 className="popup-title">Confirmar de acción</h3>
            <p className="popup-description">La acción que vas a relalizar no tiene vuelta atrás. ¿Estas seguro de que quieres llevarla a cabo?</p>
            <div>
                <button className="button-yellow w-50 mt-2 " onClick={onConfirm}>
                    Confirmar
                </button>
            </div>
        </div>
    );
};

export default ConfirmPopup;
