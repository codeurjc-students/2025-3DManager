import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { postPrint } from "../api/printService";
import { getFilamentCatalog, getPrinterCatalog, getPrintState } from "../api/catalogService";
import type { CatalogResponse } from "../models/catalog/CatalogResponse";
import { parseGcodeData } from "../models/print/GCodePatterns";

const CreatePrint3DPage: React.FC = () => {

    const [printName, setPrintName] = useState("");
    const [printFilament, setPrintFilament] = useState<number>(0);
    const [printPrinter, setPrintPrinter] = useState<number>(0);
    const [printState, setPrintState] = useState<number>(0);
    const [printDescription, setPrintDescription] = useState("");
    const [catalogState, setCatalogState] = useState<CatalogResponse[]>([]);
    const [catalogPrinter, setCatalogPrinter] = useState<CatalogResponse[]>([]);
    const [catalogFilament, setCatalogFilament] = useState<CatalogResponse[]>([]);
    const [printTime, setPrintTime] = useState<number>(0);
    const [printRealTimeH, setPrintRealTimeH] = useState<number>(0);
    const [printRealTimeM, setPrintRealTimeM] = useState<number>(0);
    const [printFilamentUsed, setPrintFilamentUsed] = useState<number>(0);
    const { user } = useAuth();

    const navigate = useNavigate();
    

    useEffect(() => {
        const loadCatalog = async () => {
            const responseCFilament = await getFilamentCatalog(user!.groupId!);
            setCatalogFilament(responseCFilament.data!);
            const responseCPrinter = await getPrinterCatalog(user!.groupId!);
            setCatalogPrinter(responseCPrinter.data!);
            const responseCState = await getPrintState();
            setCatalogState(responseCState.data!);
        };

        loadCatalog();
    }, []);

    const handleFileUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (!file) return;

        const reader = new FileReader();

        reader.onload = () => {
            const text = reader.result as string;
            

            parseGcode(text); 
        };

        reader.readAsText(file);
    };

    const parseGcode = (text: string) => {
        const { time, filament } = parseGcodeData(text);
        setPrintTime(time);
        setPrintFilamentUsed(filament);
    };



    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); 
        if (!printName || !printState || !printFilament || !printPrinter) {
            alert("Todos los campos son obligatorios");
            return;
        }
        
        try {
            let groupId = user!.groupId!;
            let userId = user!.userId;
            let printRealTime = printRealTimeH * 3600 + printRealTimeM * 60;
            const response = await postPrint({
                userId,
                printName,
                printFilament, 
                printPrinter,
                printState, 
                printDescription,
                groupId,
                printTime,
                printRealTime,
                printFilamentUsed
            });

            if (response.data) {
                alert("Impresión creada correctamente.");
                navigate("/dashboard");
            } else {
                alert(response.error?.message || "No se pudo crear la impresión.");
            }
        } catch (error) {
            console.error("Error al crear la impresión:", error);
            alert("Ha ocurrido un error en el registro de la impresión.");
        }
    };

    return (
        <div className="container-fluid vh-100">
            <div className="row h-70 mt-5">
                <div className="col-1"></div>
                <div className="grey-container col-10 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-5 mb-3">Subir pieza</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="white-container">
                            <div className="p-3 d-flex flex-column">
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-6 p-2">
                                        <label htmlFor="printName" className="form-label">Nombre</label>
                                        <input id="printName" className="input-value w-100 " value={printName} placeholder="Nombre"
                                            onChange={(e) => setPrintName(e.target.value)} />
                                    </div>
                                    <div className="col-6 p-2">
                                        <label htmlFor="printState" className="form-label">Estado</label>
                                        <select id="printState" className="input-value w-100 " value={printState}
                                            onChange={(e) => setPrintState(Number(e.target.value))}>
                                            <option value="">Seleccione un tipo</option>

                                            {catalogState.map(t => (
                                                <option key={t.id} value={t.id}>
                                                    {t.description}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                </div>
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-6 p-2">
                                        <label htmlFor="printPrinter" className="form-label">Impresora</label>
                                        <select id="printPrinter" className="input-value w-100" value={printPrinter}
                                            onChange={(e) => setPrintPrinter(Number(e.target.value))}>
                                            <option value="">Seleccione una impresora</option>

                                            {catalogPrinter.map(t => (
                                                <option key={t.id} value={t.id}>
                                                    {t.description}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                    <div className="col-6 p-2">
                                        <label htmlFor="printFilament" className="form-label">Bobina de filamento</label>
                                        <select id="printFilament" className="input-value w-100" value={printFilament}
                                            onChange={(e) => setPrintFilament(Number(e.target.value))}>
                                            <option value="">Seleccione un filamento</option>

                                            {catalogFilament.map(t => (
                                                <option key={t.id} value={t.id}>
                                                    {t.description}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                </div>
                                <div className="row-3 d-flex flex-row">
                                    <div className="col-6 p-2">
                                        <label htmlFor="printRealTimeH" className="form-label">Tiempo real impresion (Horas)</label>
                                        <input id="printRealTimeH" className="input-value w-100 " value={printRealTimeH}
                                            onChange={(e) => setPrintRealTimeH(Number(e.target.value))} />
                                    </div>
                                    <div className="col-6 p-2">
                                        <label htmlFor="printRealTimeM" className="form-label">Tiempo real impresion (Minutos)</label>
                                        <input type="number" id="printRealTimeM" className="input-value w-100 " value={printRealTimeM}
                                            onChange={(e) => setPrintRealTimeM(Number(e.target.value))} />
                                    </div>
                                </div>
                                <div className="row-3">
                                    <div className="p-2">
                                        <label htmlFor="printDescription" className="form-label">Descripción</label>
                                        <textarea id="printDescription" className="input-value w-100" value={printDescription} placeholder="Descripción"
                                            onChange={(e) => setPrintDescription(e.target.value)} />
                                    </div>
                                </div>
                            </div>

                            <div className="ms-3 me-3 p-2">
                                <label htmlFor="gcodeFile" className="form-label">Archivo GCODE</label>
                                <input
                                    type="file"
                                    id="gcodeFile"
                                    accept=".gcode,.txt"
                                    className="input-value w-100"
                                    onChange={handleFileUpload}
                                />
                            </div>
                        </div>
                        <div className="col-6 d-flex justify-content-between mt-3 p-2">
                            <button type="submit" className="botton-yellow createUser h-70">Subir Pieza</button>
                            <button type="button" className="botton-darkGrey" onClick={() => navigate("/dashboard")}>Cancelar</button>
                        </div>
                    </form>
                </div>
                <div className="col-2"></div>
            </div>
        </div>

    );
};
export default CreatePrint3DPage;


