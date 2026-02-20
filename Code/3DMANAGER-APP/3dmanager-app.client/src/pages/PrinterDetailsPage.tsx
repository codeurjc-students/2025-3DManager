import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import PrintListDetail from "../components/PrintListDetail";
import { useAuth } from "../context/AuthContext";
import { getPrinterState } from "../api/catalogService";
import type { CatalogResponse } from "../models/catalog/CatalogResponse";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import { getPrinterDetail, updatePrinter } from "../api/printerService";
import type { PrinterDetailObject } from "../models/printer/PrinterDetailObject";
import type { PrinterDetailRequest } from "../models/printer/PrinterDetailRequest";

const PrinterDetailPage: React.FC = () => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const { printerId } = useParams<{ printerId: string }>();
    const [stateData, setStateData] = useState<CatalogResponse[]>([]); 
    const [data, setData] = useState<PrinterDetailObject>(); 
    const [state, setState] = useState<number>(0);
    const [name, setName] = useState<string>("");
    const [model, setModel] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const isManager = user?.rolId === "Usuario-Manager";
    const { showPopup } = usePopupContext();


    useEffect(() => {
        getPrinterState().then(response => {
            setStateData(response.data!);
        });

        getPrinterDetail(Number(printerId)).then(response => {
            const printer = response.data;

            if (printer) {
                printer.printerCreateDate = new Date(printer.printerCreateDate);

                setData(printer);
                setState(printer.printerStateId || 0);
                setDescription(printer.printerDescription || "");
                setModel(printer.printerModel || "");
                setName(printer.printerName || "");
            }
        });
    }, []);


    const handleUpdate = async () => {
        
        try {

            const groupId = -1;
            const request: PrinterDetailRequest = {
                groupId,
                printerId: Number(printerId),
                printerName: name,
                printerDescription: description,
                printerModel: model,
                printerStateId: state
            };

            const response = await updatePrinter(request);

            if (response.data) {
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operacion realizada" description="La impresora ha sido guardado correctamente" />
                    )
                });
            } else {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operacion cancelada" description={response.error?.message || "No se pudo cambiar el estado de la impresora."} />
                    )
                });
                setState(data?.printerStateId || 0);
                
            }
        } catch (error) {
            console.error("Error al cambiar de estado de impresora", error);
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Operacion cancelada" description="Ha ocurrido un error al cambiar el estado de la impresora" />
                )
            });
        }
    };

    return (
        <div className="d-flex flex-column vh-100">
            <div className="row h-10 w-100">
                <div className="col-10">
                    <h2 className="title-impact-2 mt-3 ms-2 mb-1">Detalle de impresora</h2>
                </div>
                <div className="col-2 mt-1 ps-5">
                    <button type="button" className="white-container-button d-flex align-items-center" onClick={() => navigate("/dashboard")}>
                        <span className="dashboard-title pe-5">Volver</span>
                        <svg width="48" height="48" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M18 44V24H30V44M6 18L24 4L42 18V40C42 41.0609 41.5786 42.0783 40.8284 42.8284C40.0783 43.5786 39.0609 44 38 44H10C8.93913 44 7.92172 43.5786 7.17157 42.8284C6.42143 42.0783 6 41.0609 6 40V18Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                    </button>
                </div>
                <hr></hr>
            </div>
            <div className="row h-100">
                <div className="col-4 grey-container">
                    <div className="h-50">
                        <div className="title-impact-3 col-1 mt-2 ms-2 mb-1 w-100 d-flex flex-row justify-content-between">
                            <input type="text" className="input-value-3 me-5 w-75" value={name} disabled={!isManager}
                                onChange={(e) => setName(e.target.value)}
                            />
                            {isManager ? (
                                <button className="button-yellow ms-1 me-2" onClick={handleUpdate}>
                                <svg width="27" height="28" viewBox="0 0 27 28" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M19.125 24.5V15.1667H7.875V24.5M7.875 3.5V9.33333H16.875M21.375 24.5H5.625C5.02826 24.5 4.45597 24.2542 4.03401 23.8166C3.61205 23.379 3.375 22.7855 3.375 22.1667V5.83333C3.375 5.21449 3.61205 4.621 4.03401 4.18342C4.45597 3.74583 5.02826 3.5 5.625 3.5H18L23.625 9.33333V22.1667C23.625 22.7855 23.3879 23.379 22.966 23.8166C22.544 24.2542 21.9717 24.5 21.375 24.5Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                </svg>
                            </button>
                            ) : ""}
                        </div>
                        <div className="col-6 ms-5">
                            <img src={data?.printerImageData?.fileUrl} alt={name} className="image-container" />
                        </div>
                        <div className="col-4 mt-2 ms-3 mb-5 w-100">
                            <input type="text" className="input-value-4 me-5 mb-1 w-100" value={model} disabled={!isManager}
                                onChange={(e) => setModel(e.target.value)}/>
                            <textarea className="input-value-4 me-5 mb-1 w-100 h-08" value={description} disabled={!isManager} onChange={(e) => setDescription(e.target.value)}/>
                        </div>
                    </div>
                    <div className="h-40 ms-3 mt-1">
                        <div className="h-10 mt-2">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1 ">
                                    <label htmlFor="CreateDatePrinter" className="form-label">Fecha de alta de impresora</label>
                                    <input
                                        type="text"
                                        className="input-value-2 w-100"
                                        value={data ? data.printerCreateDate.toISOString().split("T")[0] : ""}
                                        disabled
                                    />

                                </div>
                                <div className="col-6 mb-1 ">
                                    <label htmlFor="printerModel" className="form-label">Estado</label>
                                    <div className="d-flex flex-row">
                                        <select id="printerState" className="input-value-2 w-100" value={state} disabled={!isManager} onChange={(e) => { setState(Number(e.target.value)) }}>
                                            {stateData.map(s => (
                                                <option key={s.id} value={s.id}>
                                                    {s.description}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1">
                                    <label htmlFor="printerModel" className="form-label">Horas mes actual</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printerTotalHoursMonth ?? 0} disabled />
                                </div>
                                <div className="col-6 mb-1">
                                    <label htmlFor="printerModel" className="form-label">Horas totales</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printerTotalHours ?? 0} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1">
                                    <label htmlFor="printerModel" className="form-label">Piezas impresas este mes</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printerPrintsTotalMonth ?? 0} disabled />
                                </div>
                                <div className="col-6 mb-1">
                                    <label htmlFor="printerModel" className="form-label">Piezas impresas en total</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printerPrintsTotal ?? 0} disabled />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-7 d-flex flex-column ms-auto">
                    <div className="h-35 grey-container-detail mb-2">
                    </div>
                    <div className="grey-container-detail mt-2 h-60">
                        <h3 className="title-impact-3 ms-2 mt-2">Piezas impresas</h3>
                        <PrintListDetail printerId={Number(printerId)} />
                    </div>
                </div>
            </div>


        </div> 

    );

};
export default PrinterDetailPage;

