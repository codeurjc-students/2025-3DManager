import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { postFilament } from "../api/filamentService";
import type { CatalogResponse } from "../models/catalog/CatalogResponse";
import { getFilamentType } from "../api/catalogService";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";

const CreateFilamentPage: React.FC = () => {

    const [filamentName, setFilamentName] = useState("");
    const [filamentType, setFilamentType] = useState<number>(0);
    const [filamentWeight, setFilamentWeight] = useState<number>(0);
    const [filamentColor, setFilamentColor] = useState("");
    const [filamentTemperature, setFilamentTemperature] = useState<number>(0);
    const [filamentLenght, setFilamentLenght] = useState<number>(0);
    const [filamentThickness, setFilamentThickness] = useState<number>(0);
    const [filamentCost, setFilamentCost] = useState<number>(0);
    const [filamentDescription, setFilamentDescription] = useState("");
    const [catalogTypes, setCatalogTypes] = useState<CatalogResponse[]>([]);
    const [imageFile, setImageFile] = useState<File | null>(null);

    const { showPopup } = usePopupContext();
    const navigate = useNavigate();

    useEffect(() => {
        const loadCatalog = async () => {
            const response = await getFilamentType();
            setCatalogTypes(response.data!);
        };

        loadCatalog();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); 

        if (!filamentName || !filamentType || !filamentWeight || !filamentColor || !filamentTemperature || !filamentLenght
            || !filamentThickness || !filamentCost) {
            showPopup({
                type: "warning", content: (
                    <InfoPopup title="Completar formulario" description="Todos los campos salvo la descripción son campos obligatorios" />
                )
            });

            return;
        }
        
        try {
            let groupId = -1; //It is loaded from the authentication header on API. Its not send a real value
            
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
                imageFile
            });

            if (response.data == 0) {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operación cancelada" description={response.error?.message ?? "No se pudo crear el filamento."} />
                    )
                });
            } else {
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operacion realizada" description= "Filamento creado correctamente" />
                    )
                });
                navigate("/dashboard");
            }
        } catch (error) {
            console.log( "Error al crear el filamento:" + error)
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Operación cancelada" description="No se pudo crear el filamento." />
                )
            });
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
                                        <label htmlFor="filamentType" className="form-label">Tipo Filamento</label>
                                        <select id="filamentType" className="input-value w-75" value={filamentType}
                                            onChange={(e) => setFilamentType(Number(e.target.value))}>
                                            <option value="">Seleccione un tipo</option>

                                            {catalogTypes.map(t => (
                                                <option key={t.id} value={t.id}>
                                                    {t.description}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentWeight" className="form-label">Peso Bobina</label>
                                        <input type="number" id="filamentWeight" className="input-value w-75" value={filamentWeight} 
                                            onChange={(e) => setFilamentWeight(Number(e.target.value))} />
                                    </div>
                                </div>
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-4">
                                        <label htmlFor="filamentColor" className="form-label">Color</label>
                                        <input type="color" id="filamentColor" className="input-value w-75" value={filamentColor}
                                            onChange={(e) => setFilamentColor(e.target.value)} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentTemperature" className="form-label">Temperatura ideal</label>
                                        <input type="number" id="filamentTemperature" className="input-value w-75" value={filamentTemperature} 
                                            onChange={(e) => setFilamentTemperature(Number(e.target.value))} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentLenght" className="form-label">Logitud de bobina</label>
                                        <input type="number" id="filamentLenght" className="input-value w-75" value={filamentLenght} 
                                            onChange={(e) => setFilamentLenght(Number(e.target.value))} />
                                    </div>
                                </div>
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-4">
                                        <label htmlFor="filamentThickness" className="form-label">Grosor del filamento</label>
                                        <input type="number" id="filamentThickness" className="input-value w-75" value={filamentThickness} 
                                            onChange={(e) => setFilamentThickness(Number(e.target.value))} />
                                    </div>
                                    <div className="col-4">
                                        <label htmlFor="filamentCost" className="form-label">Coste Bobina</label>
                                        <input type="number" id="filamentCost" className="input-value w-75" value={filamentCost} placeholder="€"
                                            onChange={(e) => setFilamentCost(Number(e.target.value))} />
                                    </div>
                                    <div className="col-4">
                                    </div>
                                </div>
                                <div className="row-3">
                                    <div className="mt-2">
                                        <label htmlFor="filamentDescription" className="form-label">Descripción</label>
                                        <textarea id="filamentDescription" className="input-value w-75" value={filamentDescription} placeholder="Descripción"
                                            onChange={(e) => setFilamentDescription(e.target.value)} />
                                    </div>
                                    <div className="mb-3">
                                        <label className="form-label mt-2">Imagen del filamento(Opcional)</label>
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
                            
                        </div>
                        <div className="col-6 d-flex justify-content-between mt-3 p-2">
                            <button type="submit" className="button-yellow createUser h-70">Crear filamento</button>
                            <button type="button" className="button-darkGrey" onClick={() => navigate("/dashboard")}>Cancelar</button>
                        </div>                                      
                    </form>
                </div>
                <div className="col-3"></div>
            </div>
        </div>
        
    );
};
export default CreateFilamentPage;


