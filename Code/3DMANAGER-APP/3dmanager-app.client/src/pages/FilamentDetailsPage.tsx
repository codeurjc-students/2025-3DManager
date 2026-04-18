import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import PrintListDetail from "../components/PrintListDetail";
import { useAuth } from "../context/AuthContext";
import type { CatalogResponse } from "../models/catalog/CatalogResponse";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import { getFilamentState } from "../api/catalogService";
import { deleteFilament, deleteFilamentImage, getFilamentDetail, updateFilament, updateFilamentImage } from "../api/filamentService";
import type { FilamentDetailObject } from "../models/filament/FilamentDetailObject";
import type { FilamentUpdateRequest } from "../models/filament/FilamentUpdateRequest";
import ConfirmPopup from "../components/popupComponent/ConfirmPopup";
import ImagePopup from "../components/popupComponent/ImagePopup";

const FilamentDetailPage: React.FC = () => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const [stateData, setStateData] = useState<CatalogResponse[]>([]); 
    const { filamentId } = useParams<{ filamentId: string }>();
    const [data, setData] = useState<FilamentDetailObject>(); 
    const [state, setState] = useState<number>(0);
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [temperature, setTemperature] = useState<number>(0);
    const [cost, setCost] = useState<number>(0);
    const [color, setColor] = useState<string>("");
    const [remainingLenght, setRemainingLenght] = useState<number>(0);
    const isManager = user?.rolId === "Usuario-Manager";
    const { showPopup, closePopup } = usePopupContext();
    
    useEffect(() => {
        refreshFilament();
    }, [filamentId]);


    const refreshFilament = async () => {
        const stateResponse = await getFilamentState();
        setStateData(stateResponse.data!);

        const detailResponse = await getFilamentDetail(Number(filamentId));
        const filament = detailResponse.data;

        if (filament) {
            filament.filamentCreateDate = new Date(filament.filamentCreateDate);
            setData(filament);
            setState(filament.filamentState || 0);
            setDescription(filament.filamentDescription || "");
            setName(filament.filamentName || "");
            setRemainingLenght(filament.filamentRemainingLenght || 0);
            setColor(filament.filamentColor || "");
            setTemperature(filament.filamentTemperature || 0);
            setCost(filament.filamentCost || 0);
        }
    };


    const handleUpdate = async () => {
        
        try {

            const groupId = -1;
            const request: FilamentUpdateRequest = {
                groupId,
                filamentId: Number(filamentId),
                filamentName: name,
                filamentDescription: description,
                filamentColor: color,
                filamentLenght: remainingLenght,
                filamentTemperature: temperature,
                filamentCost : cost
            };

            const response = await updateFilament(request);

            if (response.data) {
                refreshFilament();
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operacion realizada" description="El filamento ha sido guardado correctamente" />
                    )
                });
            } else {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operacion cancelada" description={response.error?.message || "No se pudo actualizar el filamento."} />
                    )
                });
                setState(data?.filamentState || 0);
            }
        } catch (error) {
            console.error("Error al actualizar el filamento", error);
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Operacion cancelada" description="Ha ocurrido un error al actualizar el filamento" />
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
                    action="Eliminar filamento"
                    onCancel={() => closePopup()}
                    onConfirm={async () => {
                        const response = await deleteFilament(Number(filamentId));

                        if (response.data) {
                            showPopup({
                                type: "info",
                                content: (
                                    <InfoPopup title="Operación realizada" description="El filamento ha sido eliminado correctamente." />
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
                                    <InfoPopup title="Error" description={response.error?.message || "No se pudo eliminar el filamento."}/>
                                ),
                                onClose: () => closePopup()
                            });
                        }
                    }}
                />
            )
        });
    };

    const openImagePopup = () => {
        showPopup({
            type: "base",
            width: "600px",
            hideCloseButton: true,
            content: (
                <ImagePopup
                    title="Actualizar imagen de filamento"
                    isSTLFile={false}
                    onUpload={async (file) => {
                        const response = await updateFilamentImage(Number(filamentId), file);

                        if (response.error) {
                            const { message } = response.error;

                            closePopup();
                            await Promise.resolve();

                            showPopup({
                                type: "error",
                                content: (<InfoPopup title="Error" description={message} />),
                                onClose: () => {
                                    closePopup();
                                }
                            });

                            return;
                        }

                        closePopup();
                        refreshFilament();
                        showPopup({
                            type: "info",
                            content: (<InfoPopup title="Imagen actualizada" description="La imagen se ha actualizado correctamente." />),
                            onClose: () => closePopup()
                            
                        });
                    }}
                    onDelete={async () => {
                        const response = await deleteFilamentImage(Number(filamentId));

                        if (response.error) {
                            const { message } = response.error;

                            closePopup();
                            await Promise.resolve();

                            showPopup({
                                type: "error",
                                content: (<InfoPopup title="Error" description={message} />
                                )
                            });

                            return;
                        }

                        closePopup();
                        refreshFilament();
                        showPopup({
                            type: "info",
                            content: (<InfoPopup title="Imagen eliminada" description="La imagen ha sido eliminada." />),
                            onClose: () => closePopup()
                            
                        });
                    }}
                    onClose={closePopup}
                />
            )
        });
    };
    return (
        <div className="d-flex flex-column vh-100">
            <div className="row h-10 w-100">
                <div className="col-10">
                    <h2 className="title-impact-2 mt-3 ms-2 mb-1">Detalle de filamento</h2>
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
                <div className="col-4 grey-container pe-4">
                    <div className="h-40">
                        <div className="title-impact-3 col-1 mt-2 ms-2 mb-1 w-100 d-flex flex-row justify-content-between">
                            <input type="text" className="input-value-3 me-5 w-75 input-editable" value={name ?? ""} disabled={!isManager}
                                onChange={(e) => setName(e.target.value)}
                            />
                            {isManager ? (
                                <div className="d-flex flex-row" >
                                    <button className="button-red" onClick={handleDelete} title="Eliminar filamento">
                                        <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M4 7.99996H6.66667M6.66667 7.99996H28M6.66667 7.99996L6.66667 26.6666C6.66667 27.3739 6.94762 28.0521 7.44772 28.5522C7.94781 29.0523 8.62609 29.3333 9.33333 29.3333H22.6667C23.3739 29.3333 24.0522 29.0523 24.5523 28.5522C25.0524 28.0521 25.3333 27.3739 25.3333 26.6666V7.99996M10.6667 7.99996V5.33329C10.6667 4.62605 10.9476 3.94777 11.4477 3.44767C11.9478 2.94758 12.6261 2.66663 13.3333 2.66663H18.6667C19.3739 2.66663 20.0522 2.94758 20.5523 3.44767C21.0524 3.94777 21.3333 4.62605 21.3333 5.33329V7.99996M13.3333 14.6666V22.6666M18.6667 14.6666V22.6666" stroke="#1E1E1E" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </button>
                                    <button className="button-yellow ms-1 me-1" onClick={handleUpdate} title="Guardar cambios">
                                        <svg width="27" height="28" viewBox="0 0 27 28" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M19.125 24.5V15.1667H7.875V24.5M7.875 3.5V9.33333H16.875M21.375 24.5H5.625C5.02826 24.5 4.45597 24.2542 4.03401 23.8166C3.61205 23.379 3.375 22.7855 3.375 22.1667V5.83333C3.375 5.21449 3.61205 4.621 4.03401 4.18342C4.45597 3.74583 5.02826 3.5 5.625 3.5H18L23.625 9.33333V22.1667C23.625 22.7855 23.3879 23.379 22.966 23.8166C22.544 24.2542 21.9717 24.5 21.375 24.5Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </button>
                                    <button className="button-yellow me-2" onClick={openImagePopup} title="Actualizar imagen">
                                        <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M6.66667 28H25.3333C26.8061 28 28 26.8061 28 25.3333V6.66667C28 5.19391 26.8061 4 25.3333 4H6.66667C5.19391 4 4 5.19391 4 6.66667V25.3333C4 26.8061 5.19391 28 6.66667 28ZM6.66667 28L21.3333 13.3333L28 20M13.3333 11.3333C13.3333 12.4379 12.4379 13.3333 11.3333 13.3333C10.2288 13.3333 9.33333 12.4379 9.33333 11.3333C9.33333 10.2288 10.2288 9.33333 11.3333 9.33333C12.4379 9.33333 13.3333 10.2288 13.3333 11.3333Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </button>
                                </div>
                            ) : ""}
                        </div>
                        <div className="col-6 ms-5 mb-3">
                            <img src={data?.filamentImageFile?.fileUrl} alt={name} className="image-container-2" />
                        </div>
                        <div className="col-4 mt-2 ms-3 w-100">
                            <textarea className="input-value-4 me-5 mb-1 w-100 h-08 input-editable" value={description ?? ""} disabled={!isManager} onChange={(e) => setDescription(e.target.value)} />
                        </div>
                    </div>
                    <div className="h-60 ms-3 mt-5">
                        <div className="h-08 mt-2">
                            <div className="d-flex flex row">
                                <div className="col-6 ">
                                    <label htmlFor="filamentCreateDatePrinter" className="form-label">Fecha de alta de filamento</label>
                                    <input
                                        id="filamentCreateDatePrinter"
                                        type="text"
                                        className="input-value-2 w-100"
                                        value={data ? data.filamentCreateDate.toISOString().split("T")[0] : ""}
                                        disabled
                                    />
                                </div>
                                <div className="col-6">
                                    <label htmlFor="filamentState" className="form-label">Estado</label>
                                    <div className="d-flex flex-row">
                                        <select id="filamentState" className="input-value-5 w-100" value={state ?? 0} disabled={!isManager} onChange={(e) => { setState(Number(e.target.value)) }}>
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
                        <div className="h-08">
                            <div className="d-flex flex row">
                                <div className="col-6">
                                    <label htmlFor="filamentColor" className="form-label">Color</label>
                                    <input id="filamentColor" type="color" className="input-value w-100 input-editable" value={color ?? ""}
                                        onChange={(e) => setColor(e.target.value)} disabled={!isManager} />
                                </div>
                                <div className="col-6">
                                    <label htmlFor="filamentTemperature" className="form-label">Temperatura ideal</label>
                                    <input id="filamentTemperature" type="text" className="input-value-5 w-100 input-editable" value={temperature ?? 0} disabled={!isManager} onChange={(e) => { setTemperature(Number(e.target.value)) }} />
                                </div>
                            </div>
                        </div>
                        <div className="h-08">
                            <div className="d-flex flex row">
                                <div className="col-6">
                                    <label htmlFor="filamentLenght" className="form-label">Longitud Original</label>
                                    <input id="filamentLenght" type="text" className="input-value-2 w-100" value={data?.filamentLenght ?? 0} disabled />
                                </div>
                                <div className="col-6">
                                    <label htmlFor="filamentCost" className="form-label">Coste</label>
                                    <input id="filamentCost" type="number" className="input-value-5 w-100 input-editable" value={cost ?? 0} disabled={!isManager} onChange={(e) => { setCost(Number(e.target.value)) }} />
                                </div>
                            </div>
                        </div>
                        <div className="h-08">
                            <div className="d-flex flex row">
                                <div className="col-6">
                                    <label htmlFor="filamentThickness" className="form-label">Grosor Filamento</label>
                                    <input id="filamentThickness" type="text" className="input-value-2 w-100" value={data?.filamentThickness ?? ""} disabled />
                                </div>
                                <div className="col-6">
                                    <label htmlFor="filamentType" className="form-label">Tipo filamento</label>
                                    <input id="filamentType" type="text" className="input-value-2 w-100" value={data?.filamentType ?? 0} disabled />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-7 d-flex flex-column ms-auto">
                    <div className="h-25 grey-container-detail mb-2">
                        <div className="d-flex flex row ms-1 mt-5">
                            <div className="col-4 ">
                                <label htmlFor="filamentPrintsLastMonth" className="form-label">Piezas impresas en el último mes</label>
                                <input id="filamentPrintsLastMonth" type="text" className="input-value-2 w-75" value={data?.filamentPrintedPrintsMonth ?? 0} disabled />
                            </div>
                            <div className="col-4 ">
                                <label htmlFor="filamentPrintsTotal" className="form-label">Piezas impresas en total</label>
                                <input id="filamentPrintsTotal" type="text" className="input-value-2 w-75" value={data?.filamentPrintedPrintsTotal ?? 0} disabled />
                            </div>
                            <div className="col-4 ">
                                <label htmlFor="filamentRemainingLenght" className="form-label">Longitud Restante</label>
                                <input id="filamentRemainingLenght" type="text" className="input-value-5 w-75 input-editable" value={remainingLenght ?? 0} disabled={!isManager} onChange={(e) => { setRemainingLenght(Number(e.target.value)) }} />
                            </div>
                        </div>
                    </div>
                    <div className="grey-container-detail mt-2 h-55">
                        <div className="h-100">
                            <h3 className="title-impact-3 ms-2 mt-2">Piezas impresas</h3>
                            <PrintListDetail id={Number(filamentId)} typeList={2} />
                        </div>
                        
                    </div>
                </div>
            </div>


        </div> 

    );

};
export default FilamentDetailPage;

