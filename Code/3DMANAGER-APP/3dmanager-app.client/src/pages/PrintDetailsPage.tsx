import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import { getPrintDetail, updatePrint } from "../api/printService";
import type { PrintDetailObject } from "../models/print/PrintDetailObject";
import type { PrintDetailRequest } from "../models/print/PrintDetailRequest";

const PrintDetailPage: React.FC = () => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const { printId } = useParams<{ printId: string }>();
    const [data, setData] = useState<PrintDetailObject>(); 
    //const [state, setState] = useState<number>(0);
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const isManager = user?.rolId === "Usuario-Manager";
    const { showPopup } = usePopupContext();


    useEffect(() => {

        getPrintDetail(Number(printId)).then(response => {
            const print = response.data;

            if (print) {
                print.printCreateDate = new Date(print.printCreateDate);
                setData(print);
                setDescription(print.printDescription || "");
                setName(print.printName || "");
            }
        });
    }, []);


    const handleUpdate = async () => {
        
        try {

            const groupId = -1;
            const request: PrintDetailRequest = {
                groupId,
                printId: Number(printId),
                printName: name,
                printDescription: description,

            };

            const response = await updatePrint(request);

            if (response.data) {
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operacion realizada" description="La impresión ha sido guardado correctamente" />
                    )
                });
            } else {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operacion cancelada" description={response.error?.message || "No se pudo actualizar de la impresión."} />
                    )
                });
                
            }
        } catch (error) {
            console.error("Error al actualizar la impresión", error);
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Operacion cancelada" description="Ha ocurrido un error al actualizar la impresión" />
                )
            });
        }
    };

    return (
        <div className="d-flex flex-column vh-100">
            <div className="row h-10 w-100">
                <div className="col-10">
                    <h2 className="title-impact-2 mt-3 ms-2 mb-1">Detalles de Pieza</h2>
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
                            <img src={data?.printImageData?.fileUrl} alt={name} className="image-container-3" />
                        </div>
                    </div>
                    <div className="h-40 ms-3 mt-1">
                        <div className="h-10 mt-2">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1 "></div>
                                <div className="col-6 mb-1">
                                    <label htmlFor="printUser" className="form-label">Usuario</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printUserName}
                                        onClick={() => navigate(`/dashboard/user/detail/${data?.printUserId}`)} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1">
                                    <label htmlFor="printPrinter" className="form-label">Impresora</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printPrinterName}
                                        onClick={() => navigate(`/dashboard/print/detail/${data?.printPrinterId}`)} disabled />
                                </div>
                                <div className="col-6 mb-1">
                                    <label htmlFor="printFilament" className="form-label">Filamento</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printFilamentName}
                                        onClick={() => navigate(`/dashboard/filament/detail/${data?.printFilamentName}`)} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1">
                                    <label htmlFor="printMaterial" className="form-label">Material</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printMaterial} disabled />
                                </div>
                                <div className="col-6 mb-1 ">
                                    <label htmlFor="CreateDatePrint" className="form-label">Fecha de alta de impresión</label>
                                    <input
                                        type="text"
                                        className="input-value-2 w-100"
                                        value={data ? data.printCreateDate.toISOString().split("T")[0] : ""}
                                        disabled
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-7 d-flex flex-column ms-auto">
                    <div className="h-35 grey-container-detail mb-2">
                        <h3 className="title-impact-3 ms-3 mt-1">Datos impresión</h3>
                        <div className="h-08 ms-3">
                            <div className="d-flex flex row">
                                <div className="col-5 ">
                                    <label htmlFor="printTime" className="form-label">Tiempo impresión</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printTimeImpression} disabled />
                                </div>
                                <div className="col-5 ">
                                    <label htmlFor="printRealTime" className="form-label">Tiempo real impresión</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printRealTimeImpression} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-08 ms-3">
                            <div className="d-flex flex row">
                                <div className="col-5">
                                    <label htmlFor="printMaterialConsumed" className="form-label">Material usado</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printMaterialConsumed} disabled />
                                </div>
                                <div className="col-5 ">
                                    <label htmlFor="printEstimatedCost" className="form-label">Estimación de coste</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printEstimedCost} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-10 ms-3">
                            <div className="d-flex flex row">
                                <div className="col-10">
                                    <label htmlFor="printDescription" className="form-label">Descripcion</label>
                                    <textarea className="input-value-5 w-100" value={description}
                                        onChange={(e) => setDescription(e.target.value)} disabled={!isManager} />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="grey-container-detail mt-2 h-50">
                        <h3 className="title-impact-3 ms-2 mt-2">Comentarios</h3>
                       {/* <PrintComments id={Number(printId)}/>*/}
                    </div>
                </div>
            </div>


        </div> 

    );

};
export default PrintDetailPage;

