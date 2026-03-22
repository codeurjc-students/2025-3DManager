import React from "react";

interface ConfirmPopup {
    action: string;
    onConfirm: () => void;
    onCancel: () => void;
}

const ConfirmPopup: React.FC<ConfirmPopup> = ({action, onConfirm, onCancel}) => {
    return (
        <div className="confirm-popup">
            <h3 className="popup-title">Confirmar de acción: {action}</h3>
            <p className="popup-description">La acción que vas a realizar no tiene vuelta atrás. ¿Estas seguro de que quieres llevarla a cabo?</p>
            <div className="d-flex flex-row">
                <button className="button-yellow w-50 me-2" onClick={onConfirm}>
                    Confirmar
                </button>
                <button className="button-darkGrey w-50 ms-2" onClick={onCancel}>
                    Cancelar
                </button>
            </div>
        </div>
    );
};

export default ConfirmPopup;
