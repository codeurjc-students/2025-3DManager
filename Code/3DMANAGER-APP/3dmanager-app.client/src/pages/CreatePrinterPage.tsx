import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { postPrinter } from "../api/printerService";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";

const CreatePrinterPage: React.FC = () => {

    const [printerName, setPrinterName] = useState("");
    const [printerDescription, setPrinterDescription] = useState("");
    const [printerModel, setPrinterModel] = useState("");
    const [imageFile, setImageFile] = useState<File | null>(null);
    const { showPopup } = usePopupContext();

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); 
        if (!printerName || !printerModel) {
            showPopup({
                type: "warning", content: (
                    <InfoPopup title="Completar formulario" description="El nombre y el modelo de la impresora son campos obligatorios" />
                )
            });
            return;
        }
        
        try {

            const response = await postPrinter({
                printerName,
                printerDescription,
                printerModel,
                groupId: -1,
                imageFile
            });

            if (response.data) {
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operación realizada" description="La impresora ha sido creada correctamente." />
                    )
                });
                navigate("/dashboard");
            } else {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operación cancelada" description={response.error?.message || "No se ha podido crear la impresora."} />
                    )
                });
            }
        } catch (error) {
            console.error("Error al crear impresora:", error);
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Operación cancelada" description= "Ha ocurrido un error en la creación de la impresora."/>
                )
            });
        }
    };

    return (
        <div className="container-fluid vh-100">         
            <div className="row h-50 mt-5">
                <div className="col-3"></div>
                <div className="grey-container col-6 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-5 mb-5">Crear impresora</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="white-container">
                            <div className="p-3">
                                <div className="mb-3">
                                    <label htmlFor="printerName" className="form-label">Nombre de impresora</label>
                                    <input id="printerName" type="text" className="input-value w-75" value={printerName} placeholder="Nombre impresora"
                                        onChange={(e) => setPrinterName(e.target.value)} />
                                </div>
                                <div className="mb-3">
                                    <label htmlFor="printerModel" className="form-label">Modelo de la impresora</label>
                                    <input id="printerModel" className="input-value w-75" value={printerModel} placeholder="Modelo impresora"
                                        onChange={(e) => setPrinterModel(e.target.value)} />
                                </div>
                                <div className="mb-3">
                                    <label htmlFor="printerDescription" className="form-label">Descripción</label>
                                    <textarea id="printerDescription" className="input-value w-75" value={printerDescription} placeholder="Descripción"
                                        onChange={(e) => setPrinterDescription(e.target.value)} />
                                </div>
                                <div className="mb-3">
                                    <label className="form-label">Imagen de la impresora</label>
                                    <input
                                        type="file"
                                        className="form-control w-75"
                                        accept="image/*"
                                        onChange={(e) => {
                                            if (e.target.files && e.target.files.length > 0) {
                                                setImageFile(e.target.files[0]);
                                            }
                                        }}
                                    />
                                </div>
                            </div>                           
                        </div>
                        <div className="col-6 d-flex justify-content-between mt-5 p-2">
                            <button type="submit" className="button-yellow createUser h-70">Crear impresora</button>
                            <button type="button" className="button-darkGrey" onClick={() => navigate("/dashboard")}>Cancelar</button>
                        </div>                                      
                    </form>
                </div>
                <div className="col-3"></div>
            </div>
        </div>
        
    );
};
export default CreatePrinterPage;


