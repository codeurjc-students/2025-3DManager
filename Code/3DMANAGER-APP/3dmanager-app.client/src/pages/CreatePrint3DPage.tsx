import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { postPrint } from "../api/printService";
import { getFilamentCatalog, getPrinterCatalog, getPrintState } from "../api/catalogService";
import type { CatalogResponse } from "../models/catalog/CatalogResponse";
import { parseGcodeData } from "../models/print/GCodePatterns";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import type { CatalogPrinterResponse } from "../models/catalog/CatalogPrinterResponse";

const CreatePrint3DPage: React.FC = () => {

    const [printName, setPrintName] = useState("");
    const [printFilament, setPrintFilament] = useState<number>(0);
    const [printPrinter, setPrintPrinter] = useState<number>(0);
    const [printPrinterEstimation, setPrintPrinterEstimation] = useState<number>(0);
    const [printState, setPrintState] = useState<number>(0);
    const [printDescription, setPrintDescription] = useState("");
    const [catalogState, setCatalogState] = useState<CatalogResponse[]>([]);
    const [catalogPrinter, setCatalogPrinter] = useState<CatalogPrinterResponse[]>([]);
    const [catalogFilament, setCatalogFilament] = useState<CatalogResponse[]>([]);
    const [printTime, setPrintTime] = useState<number>(0);
    const [printTimeEstimation, setPrintTimeEstimation] = useState<number>(0);
    const [printRealTimeH, setPrintRealTimeH] = useState<number>(0);
    const [printRealTimeM, setPrintRealTimeM] = useState<number>(0);
    const [printFilamentUsed, setPrintFilamentUsed] = useState<number>(0);
    const [imageFile, setImageFile] = useState<File | null>(null);
    const [printProgress, setPrintProgress] = useState<number>(0);

    const { showPopup } = usePopupContext();
    const navigate = useNavigate();


    useEffect(() => {
        const loadCatalog = async () => {
            const responseCFilament = await getFilamentCatalog();
            const filaments = responseCFilament.data ?? [];
            setCatalogFilament(filaments);
            if (filaments.length === 0) {
                showPopup({
                    type: "warning",
                    content: (
                        <InfoPopup
                            title="Sin filamentos"
                            description="No hay bobinas de filamento registradas. Debe añadir una antes de crear una impresión."
                        />
                    )
                });
            }

            const responseCPrinter = await getPrinterCatalog();
            const printers = responseCPrinter.data ?? [];
            setCatalogPrinter(printers);
            if (printers.length === 0) {
                showPopup({
                    type: "warning",
                    content: (
                        <InfoPopup
                            title="Sin impresoras"
                            description="No hay impresoras registradas. Debe añadir una antes de crear una impresión."
                        />
                    )
                });
            }

            const responseCState = await getPrintState();
            const states = responseCState.data ?? [];
            setCatalogState(states);
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

    const handleFileUploadEstimation = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (!file) return;

        const reader = new FileReader();

        reader.onload = () => {
            const text = reader.result as string;


            parseGcodeEstimation(text);
        };

        reader.readAsText(file);
    };

    const parseGcode = (text: string) => {
        const { time, filament } = parseGcodeData(text);
        setPrintTime(time);
        setPrintFilamentUsed(filament);
    };

    const parseGcodeEstimation = (text: string) => {
        const { time } = parseGcodeData(text);
        setPrintTimeEstimation(time);
    };

    const formatSeconds = (totalSeconds: number) => {
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = Math.floor(totalSeconds % 60);

        const parts = [];
        if (hours > 0) parts.push(`${hours} h`);
        if (minutes > 0) parts.push(`${minutes} min`);
        if (seconds > 0 || parts.length === 0) parts.push(`${seconds} s`);

        return parts.join(" ");
    }
    const selectedPrinter = catalogPrinter.find(p => p.id === printPrinterEstimation);

    let estimationBlock = null;

    if (printTimeEstimation && selectedPrinter) {
        const slicerSeconds = printTimeEstimation; 
        const variation = selectedPrinter.timeVariation;
        const absVariation = Math.abs(variation).toFixed(2);
        const adjustedSeconds = slicerSeconds + (slicerSeconds * variation / 100);

        let variationColor;
        let variationText = "";

        if (variation < 0) {
            variationColor = "green";
            variationText = `${absVariation}% más rápido`;
        } else if (variation > 0) {
            variationColor = "red";
            variationText = `${absVariation}% más lento`;
        } else {
            variationColor = "#555";
            variationText = "No hay histórico suficiente para estimar variación";
        }

        estimationBlock = (
            <div className="mt-3 p-3 rounded" style={{ background: "#f7f7f7" }}>
                {variation === 0 ? (
                    <p style={{ color: variationColor }}>No hay histórico suficiente para estimar variación.</p>
                ) : (
                    <p>El tiempo de impresión es{" "}<strong style={{ color: variationColor }}>
                        {variationText}</strong>{" "}según el histórico.
                    </p>
                )}
                <p>La pieza tardará aproximadamente{" "}<strong>{formatSeconds(adjustedSeconds)}</strong>.</p>
                <p>El tiempo estimado por el laminador es{" "}<strong>{formatSeconds(slicerSeconds)}</strong>.</p>
            </div>
        );
    }


    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!printName || !printState || !printFilament || !printPrinter || !printDescription || !printTime || !printRealTimeM) {
            showPopup({
                type: "warning", content: (
                    <InfoPopup title="Completar formulario" description="Debe rellenar todos los campos salvo la imagen de impresión" />
                )
            });
            return;
        }
        
        try {
            let groupId = -1;
            let userId = -1;
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
                printFilamentUsed,
                printProgress,
                imageFile
            });

            if (response.data) {
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operacion realizada" description="La impresión ha sido guardada correctamente" />
                    )
                });
                navigate("/dashboard");
            } else {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operacion cancelada" description={response.error?.message || "No se pudo crear la impresión."} />
                    )
                });
            }
        } catch (error) {
            console.error("Error al crear la impresión:", error);
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Operacion cancelada" description="Ha ocurrido un error en el registro de la impresión." />
                )
            });
        }
    };

    return (
        <div className="container-fluid vh-100">
            <div className="row h-75 mt-5">
                <div className="col-3 me-3">
                    
                    <div className="grey-container h-75">
                        <h2 className="title-impact-2 mt-2 ms-3 mb-2">Obtener Estimación</h2>
                        <p className="ms-3">Agrega el fichero .gcode antes de la impresión y selecciona la impresora para obtener un tiempo estimado real en base
                        al histórico</p>
                        <div className="ms-3 me-3 p-2">
                            <label htmlFor="gcodeFileEstimation" className="form-label">Archivo GCODE para estimacion</label>
                            <input
                                type="file"
                                id="gcodeFileEstimation"
                                accept=".gcode,.txt"
                                className="input-value w-100"
                                onChange={handleFileUploadEstimation}
                            />
                            <div className="mt-2">
                                <label htmlFor="printPrinterEstimation" className="form-label">Impresora para estimar tiempo</label>
                                <select id="printPrinterEstimation" className="input-value w-100" value={printPrinterEstimation}
                                    onChange={(e) => setPrintPrinterEstimation(Number(e.target.value))}>
                                    <option value={0}>Seleccione una impresora para la estimación</option>
                                    {catalogPrinter.map(t => (
                                        <option key={t.id} value={t.id}>
                                            {t.description}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            {estimationBlock}
                        </div> 
                    </div>
                </div>
                <div className="grey-container col-8 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-2 mb-2">Subir pieza</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="white-container">
                            <div className="p-2 d-flex flex-column">
                                <div className="row-3 d-flex flex-row">
                                    <div className={printState === 2 ? "col-4 p-2" : "col-6 p-2"}>
                                        <label htmlFor="printName" className="form-label">Nombre</label>
                                        <input id="printName" className="input-value w-100" value={printName} placeholder="Nombre" onChange={(e) => setPrintName(e.target.value)}/>
                                    </div>
                                    <div className={printState === 2 ? "col-4 p-2" : "col-6 p-2"}>
                                        <label htmlFor="printState" className="form-label">Estado</label>
                                        <select id="printState" className="input-value w-100" value={printState} onChange={(e) => setPrintState(Number(e.target.value))}>
                                            <option value={0}>Seleccione un estado</option>
                                            {catalogState.map(t => (
                                                <option key={t.id} value={t.id}>{t.description}</option>
                                            ))}
                                        </select>
                                    </div>

                                    {printState === 2 && (
                                        <div className="col-4 p-2">
                                            <label htmlFor="printProgress" className="form-label">
                                                Porcentaje completado antes del fallo
                                            </label>
                                            <input id="printProgress" type="number" className="input-value w-100" min={0} max={100}
                                                value={printProgress} onChange={(e) => setPrintProgress(Number(e.target.value))} placeholder="0 - 100"/>
                                        </div>
                                    )}
                                </div>

                                <div className="row-3 d-flex flex-row">
                                    <div className="col-6 p-2">
                                        <label htmlFor="printPrinter" className="form-label">Impresora</label>
                                        <select id="printPrinter" className="input-value w-100" value={printPrinter}
                                            onChange={(e) => setPrintPrinter(Number(e.target.value))}>
                                            <option value={0}>Seleccione una impresora</option>
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
                                            <option value={0}>Seleccione un filamento</option>
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
                                        <label htmlFor="printRealTimeH" className="form-label">Tiempo real impresión (Horas)</label>
                                        <input id="printRealTimeH" className="input-value w-100 " value={printRealTimeH}
                                            onChange={(e) => setPrintRealTimeH(Number(e.target.value))} />
                                    </div>
                                    <div className="col-6 p-2">
                                        <label htmlFor="printRealTimeM" className="form-label">Tiempo real impresión (Minutos)</label>
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
                            <div className="ms-3 me-3 p-2">
                                <label htmlFor="PrintSTL" className="form-label">STL de la impresión</label>
                                <input
                                    id="PrintSTL"
                                    type="file"
                                    className="form-control input-value w-100"
                                    accept=".stl"
                                    onChange={(e) => {
                                        if (e.target.files && e.target.files.length > 0) {
                                            setImageFile(e.target.files[0]);
                                        }
                                    }}
                                />
                            </div>
                        </div>
                        <div className="col-4 d-flex justify-content-between mt-3 p-2">
                            <button type="submit" className="button-yellow createUser h-70">Subir Pieza</button>
                            <button type="button" className="button-darkGrey" onClick={() => navigate("/dashboard")}>Cancelar</button>
                        </div>
                    </form>
                </div>
                <div className="col-1"></div>
            </div>
        </div>

    );
};
export default CreatePrint3DPage;


