import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { postPrinter } from "../api/printerService";

const CreatePrinterPage: React.FC = () => {

    const [printerName, setPrinterName] = useState("");
    const [printerDescription, setPrinterDescription] = useState("");
    const [printerModel, setPrinterModel] = useState("");
    const [imageFile, setImageFile] = useState<File | null>(null);

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); 
        if (!printerName || !printerModel) {
            alert("El nombre y el modelo son campos obligatorios");
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
                alert("Impresora creada correctamente.");
                navigate("/dashboard");
            } else {
                alert(response.error?.message || "No se pudo crear la impresora.");
            }
        } catch (error) {
            console.error("Error al crear impresora:", error);
            alert("Ha ocurrido un error en el registro del impresora.");
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
                            <button type="submit" className="botton-yellow createUser h-70">Crear impresora</button>
                            <button type="button" className="botton-darkGrey" onClick={() => navigate("/dashboard")}>Cancelar</button>
                        </div>                                      
                    </form>
                </div>
                <div className="col-3"></div>
            </div>
        </div>
        
    );
};
export default CreatePrinterPage;


