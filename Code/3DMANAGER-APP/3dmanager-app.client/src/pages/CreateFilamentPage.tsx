import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { postPrinter } from "../api/printerService";
import { postFilament } from "../api/filamentService";

const CreateFilamentPage: React.FC = () => {

    const [filamentName, setFilamentName] = useState("");
    const [filamentType, setFilamentType] = useState(1);
    const [filamentWeight, setFilamentWeight] = useState(0);
    const [filamentColor, setFilamentColor] = useState("");
    const [filamentTemperature, setFilamentTemperature] = useState(0);
    const [filamentLenght, setFilamentLenght] = useState(0);
    const [filamentThickness, setFilamentThickness] = useState(0);
    const [filamentCost, setFilamentCost] = useState(0);
    const [filamentDescription, setFilamentDescription] = useState("");

    const { user } = useAuth();

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); // Para no recargar la página

        if (!filamentName || !filamentType || !filamentWeight || !filamentColor || !filamentTemperature || !filamentLenght
            || !filamentThickness || !filamentCost ) {
            alert("Todos los campos salvo la descripción son campos obligatorios");
            return;
        }
        
        try {
            let groupId = user!.groupId!;
            // Llamada al servicio
            const response = await postFilament({
                groupId,
                filamentName, 
                filamentType, 
                filamentWeight, 
                filamentColor, 
                filamentTemperature,
                filamentLenght, 
                filamentThickness, 
                filamentCost, 
                filamentDescription,
            });

            if (response.data) {
                alert("Filamento creado correctamente.");
                navigate("/dashboard");
            } else {
                alert(response.error?.message || "No se pudo crear el filamento.");
            }
        } catch (error) {
            console.error("Error al crear el filamento:", error);
            alert("Ha ocurrido un error en el registro del filamento.");
        }
    };

    return (
        <div className="container-fluid vh-100">         
            <div className="row h-70 mt-5">
                <div className="col-2"></div>
                <div className="grey-container col-8 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-5 mb-3">Crear filamento</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="white-container">
                            <div className="p-3 d-flex flex-column">
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                </div>
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                </div>
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentName" className="form-label">Nombre</label>
                                        <input id="filamentName" className="input-value w-75" value={filamentName} placeholder="Nombre"
                                            onChange={(e) => setFilamentName(e.target.value)} />
                                    </div>
                                    <div className="col-4">
                                    </div>
                                </div>
                                <div className="row-3">
                                    <div className="">
                                        <label htmlFor="filamentDescription" className="form-label">Descripción</label>
                                        <textarea id="filamentDescription" className="input-value w-75" value={filamentDescription} placeholder="Descripción"
                                            onChange={(e) => setFilamentDescription(e.target.value)} />
                                    </div>
                                </div>
                            </div>                           
                        </div>
                        <div className="col-6 d-flex justify-content-between mt-3 p-2">
                            <button type="submit" className="botton-yellow createUser h-70">Crear filamento</button>
                            <button type="button" className="botton-darkGrey" onClick={() => navigate("/dashboard")}>Cancelar</button>
                        </div>                                      
                    </form>
                </div>
                <div className="col-3"></div>
            </div>
        </div>
        
    );
};
export default CreateFilamentPage;


