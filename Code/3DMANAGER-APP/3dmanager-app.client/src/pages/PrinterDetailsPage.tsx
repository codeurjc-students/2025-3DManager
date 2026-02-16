import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import PrintListDetail from "../components/PrintListDetail";
import { useAuth } from "../context/AuthContext";
import { getPrinterState } from "../api/catalogService";
import type { CatalogResponse } from "../models/catalog/CatalogResponse";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import { updatePrinterState } from "../api/printerService";

const PrinterDetailPage: React.FC = () => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const { printerId } = useParams<{ printerId: string }>();
    const [stateData, setStateData] = useState<CatalogResponse[]>([]);
    const [state, setState] = useState<number>(0);
    const isManager = user?.rolId === "Usuario-Manager";
    const { showPopup } = usePopupContext();

    useEffect(() => {

        getPrinterState().then(response => {
            setStateData(response.data!);
        });

    }, []);

    const handleSetState = async () => {
        showPopup({
            type: "info", content: (
                <InfoPopup title="Operacion realizada" description="La impresora ha sido cambiada de estado " />
            )
        });
        try {
            const response = await updatePrinterState(Number(printerId),state);

            if (response.data) {
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operacion realizada" description="El estado de la impresora ha sido guardado correctamente" />
                    )
                });
            } else {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operacion cancelada" description={response.error?.message || "No se pudo cambiar el estado de la impresora."} />
                    )
                });
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
                    <button type="button" className="white-container-button d-flex align-items-center" disabled={ !isManager} onClick={() => navigate("/dashboard")}>
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
                    <div className="h-40">
                    </div>
                    <div className="h-60 ms-5 mt-5">
                        <div className="h-10 mt-2">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-3 ">
                                    <label htmlFor="CreateDatePrinter" className="form-label">Fecha de alta de impresora</label>
                                    <input type="date" className="input-value-2 w-75" value={1} disabled />
                                </div>
                                <div className="col-6 mb-3">
                                    <label htmlFor="printerModel" className="form-label">Estado</label>
                                    <select id="printerState" className="input-value w-75" value={state} onChange={(e) => { setState(Number(e.target.value)); handleSetState() }}>
                                        
                                        {stateData.map(s => (
                                            <option key={s.id} value={s.id}>
                                                {s.description}
                                                </option>
                                        ))}
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-3">
                                    <label htmlFor="printerModel" className="form-label">Horas mes actual</label>
                                    <input type="text" className="input-value-2 w-75" value={1} disabled />
                                </div>
                                <div className="col-6 mb-3">
                                    <label htmlFor="printerModel" className="form-label">Horas totales</label>
                                    <input type="text" className="input-value-2 w-75" value={1} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-3">
                                    <label htmlFor="printerModel" className="form-label">Piezas impresas este mes</label>
                                    <input type="text" className="input-value-2 w-75" value={1} disabled />
                                </div>
                                <div className="col-6 mb-3">
                                    <label htmlFor="printerModel" className="form-label">Piezas impresas en total</label>
                                    <input type="text" className="input-value-2 w-75" value={1} disabled />
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

