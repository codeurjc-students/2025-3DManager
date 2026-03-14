import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import { deletePrint, getPrintDetail, updatePrint } from "../api/printService";
import type { PrintDetailObject } from "../models/print/PrintDetailObject";
import type { PrintDetailRequest } from "../models/print/PrintDetailRequest";
import PrintComments from "../components/PrintComments";
import ConfirmPopup from "../components/popupComponent/ConfirmPopup";

const PrintDetailPage: React.FC = () => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const { showPopup, closePopup } = usePopupContext();
    const { printId } = useParams<{ printId: string }>();
    const [data, setData] = useState<PrintDetailObject>(); 
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const isManagerOrOwner = user?.rolId === "Usuario-Manager" || data?.printUserId === user?.userId;


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

    const handleDelete = () => {
        showPopup({
            type: "base",
            hideCloseButton: true,
            content: (
                <ConfirmPopup
                    action="Eliminar impresión 3d"
                    onCancel={() => closePopup()}
                    onConfirm={async () => {
                        const response = await deletePrint(Number(printId));

                        if (response.data) {
                            showPopup({
                                type: "info",
                                content: (
                                    <InfoPopup
                                        title="Operación realizada"
                                        description="La impresión 3d ha sido eliminada correctamente."
                                    />
                                ),
                                onClose: () => {
                                    closePopup();
                                    navigate("/dashboard");
                                }
                            });
                        } else {
                            showPopup({
                                type: "error",
                                content: (
                                    <InfoPopup
                                        title="Error"
                                        description={response.error?.message || "No se pudo eliminar la impresión 3d."}
                                    />
                                ),
                                onClose: () => closePopup()
                            });
                        }
                    }}
                />
            )
        });
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
                            <input type="text" className="input-value-3 me-5 w-75" value={name} disabled={!isManagerOrOwner}
                                onChange={(e) => setName(e.target.value)}
                            />
                            {isManagerOrOwner ? (
                                <div className="d-flex flex-row" >
                                    <button className="button-red" onClick={handleDelete}>
                                        <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M4 7.99996H6.66667M6.66667 7.99996H28M6.66667 7.99996L6.66667 26.6666C6.66667 27.3739 6.94762 28.0521 7.44772 28.5522C7.94781 29.0523 8.62609 29.3333 9.33333 29.3333H22.6667C23.3739 29.3333 24.0522 29.0523 24.5523 28.5522C25.0524 28.0521 25.3333 27.3739 25.3333 26.6666V7.99996M10.6667 7.99996V5.33329C10.6667 4.62605 10.9476 3.94777 11.4477 3.44767C11.9478 2.94758 12.6261 2.66663 13.3333 2.66663H18.6667C19.3739 2.66663 20.0522 2.94758 20.5523 3.44767C21.0524 3.94777 21.3333 4.62605 21.3333 5.33329V7.99996M13.3333 14.6666V22.6666M18.6667 14.6666V22.6666" stroke="#1E1E1E" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"/>
                                        </svg>
                                    </button>
                                    <button className="button-yellow ms-1 me-2" onClick={handleUpdate}>
                                        <svg width="27" height="28" viewBox="0 0 27 28" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M19.125 24.5V15.1667H7.875V24.5M7.875 3.5V9.33333H16.875M21.375 24.5H5.625C5.02826 24.5 4.45597 24.2542 4.03401 23.8166C3.61205 23.379 3.375 22.7855 3.375 22.1667V5.83333C3.375 5.21449 3.61205 4.621 4.03401 4.18342C4.45597 3.74583 5.02826 3.5 5.625 3.5H18L23.625 9.33333V22.1667C23.625 22.7855 23.3879 23.379 22.966 23.8166C22.544 24.2542 21.9717 24.5 21.375 24.5Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </button>
                                </div>
                            ) : ""}
                        </div>
                        <div className="col-6 ms-5">
                            <img src={data?.printImageData?.fileUrl} alt={name} className="image-container-3" />
                        </div>
                    </div>
                    <div className="h-40 ms-3 mt-1">
                        <div className="h-10 mt-2">
                            <div className="d-flex flex row">
                                <div className="col-6 mt-4">
                                    {data?.printState == 1 ?
                                        <span className="status-badge status-active">Finalizada</span> :
                                        <span className="status-badge status-maintenance">No finalizada</span>}
                                </div>
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
                    <div className="h-30 grey-container-detail mb-2">
                        <h3 className="title-impact-3 ms-3 mt-1">Datos impresión</h3>
                        <div className="h-08 ms-3 me-3">
                            <div className="d-flex flex row">
                                <div className="col-3 ">
                                    <label htmlFor="printTime" className="form-label">Tiempo impresión</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printTimeImpression} disabled />
                                </div>
                                <div className="col-3 ">
                                    <label htmlFor="printRealTime" className="form-label">Tiempo real impresión</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printRealTimeImpression} disabled />
                                </div>
                                <div className="col-3">
                                    <label htmlFor="printMaterialConsumed" className="form-label">Material usado</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printMaterialConsumed} disabled />
                                </div>
                                <div className="col-3">
                                    <label htmlFor="printEstimatedCost" className="form-label">Estimación de coste</label>
                                    <input type="text" className="input-value-2 w-100" value={data?.printEstimedCost} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-25 ms-3 me-3">
                            <div className="d-flex flex-rows ">
                                <div className="col-10 w-100">
                                    <label htmlFor="printDescription" className="form-label">Descripcion</label>
                                    <textarea className="input-value-5 table-scroll w-100" value={description}
                                        onChange={(e) => setDescription(e.target.value)} disabled={!isManagerOrOwner} />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="grey-container-detail mt-2 h-55">
                        <h3 className="title-impact-3 ms-3 mt-2">Comentarios</h3>
                        <PrintComments id={Number(printId)}/>
                    </div>
                </div>
            </div>


        </div> 

    );

};
export default PrintDetailPage;

