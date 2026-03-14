import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import PrintListDetail from "../components/PrintListDetail";
import { useAuth } from "../context/AuthContext";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import type { UserDetailObject } from "../models/user/UserDetailObject";
import type { UserUpdateRequest } from "../models/user/UserUpdateRequest";
import { getUserDetail, updateUser } from "../api/userService";


const UserDetailPage: React.FC = () => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const { userId } = useParams<{ userId: string }>();
    const [data, setData] = useState<UserDetailObject>();
    const [name, setName] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const isMyUser = user?.userId == Number(userId) && user?.rolId != "Usuario-Invitado";
    const { showPopup } = usePopupContext();


    useEffect(() => {
        getUserDetail(Number(userId)).then(response => {
            const user = response.data;

            if (user) {
                user.userCreateDate = new Date(user.userCreateDate);
                setData(user);
                setName(user.userName || "");
                setEmail(user.userEmail || "");
            }
        });
    }, []);


    const handleUpdate = async () => {
        try {

            const request: UserUpdateRequest = {
                userGroupId : - 1,
                userId: Number(userId),
                userName: name,
                userEmail: email,
            };
            const response = await updateUser(request);

            if (response.data) {
                showPopup({
                    type: "info", content: (
                        <InfoPopup title="Operacion realizada" description="Tú usuario ha sido modificado correctamente" />
                    )
                });
            } else {
                showPopup({
                    type: "error", content: (
                        <InfoPopup title="Operacion cancelada" description={response.error?.message || "No se pudo cambiar los datos de usuario."} />
                    )
                });
            }
        } catch (error) {
            console.error("Error al cambiar el perfil del usuario", error);
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Operacion cancelada" description="Ha ocurrido un error al cambiar el perfil del usuario" />
                )
            });
        }
    };

    return (
        <div className="d-flex flex-column vh-100">
            <div className="row h-10 w-100">
                <div className="col-10">
                    <h2 className="title-impact-2 mt-3 ms-2 mb-1">Detalle de usuario</h2>
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
                    <div className="h-40">
                        <div className="title-impact-3 col-1 mt-2 ms-2 mb-1 w-100 d-flex flex-row justify-content-between">
                            <input type="text" className="input-value-3 me-5 w-75" value={name} disabled={!isMyUser}
                                onChange={(e) => setName(e.target.value)}
                            />
                            {isMyUser ? (
                                <button className="button-yellow ms-1 me-2" onClick={handleUpdate}>
                                    <svg width="27" height="28" viewBox="0 0 27 28" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M19.125 24.5V15.1667H7.875V24.5M7.875 3.5V9.33333H16.875M21.375 24.5H5.625C5.02826 24.5 4.45597 24.2542 4.03401 23.8166C3.61205 23.379 3.375 22.7855 3.375 22.1667V5.83333C3.375 5.21449 3.61205 4.621 4.03401 4.18342C4.45597 3.74583 5.02826 3.5 5.625 3.5H18L23.625 9.33333V22.1667C23.625 22.7855 23.3879 23.379 22.966 23.8166C22.544 24.2542 21.9717 24.5 21.375 24.5Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                    </svg>
                                </button>
                            ) : ""}
                        </div>
                        <div className="title-impact-3 col-2 mt-2 ms-2 mb-1 w-100 d-flex flex-row justify-content-between">
                            <p className="input-value-4">{data?.userRole}</p>
                        </div>
                        <div className=" h-30 col-6 ms-5">
                            <img src={data?.userImageData?.fileUrl} alt={name} className="image-container" />
                        </div>
                    </div>
                    <div className="h-60 ms-3 mt-4">
                        <div className="h-10 ">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1 ">
                                    <label htmlFor="CreateDateUser" className="form-label">Fecha de alta de usuario</label>
                                    <input
                                        id="CreateDateUser"
                                        type="text"
                                        className="input-value-2 w-100"
                                        value={data ? data.userCreateDate.toISOString().split("T")[0] : ""}
                                        disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-12 mb-1 ">
                                    <label htmlFor="Email" className="form-label">Email</label>
                                    <input id="Email" type="text" className="input-value-5 me-5 w-100" value={email} disabled={!isMyUser}
                                        onChange={(e) => setEmail(e.target.value)}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1">
                                    <label htmlFor="HoursMonth" className="form-label">Horas mes actual</label>
                                    <input id="HoursMonth" type="text" className="input-value-2 w-100" value={data?.userPrintHours ?? 0} disabled />
                                </div>
                                <div className="col-6 mb-1">
                                    <label htmlFor="HoursTotal" className="form-label">Horas totales</label>
                                    <input id="HoursTotal" type="text" className="input-value-2 w-100" value={data?.userTotalHours ?? 0} disabled />
                                </div>
                            </div>
                        </div>
                        <div className="h-10">
                            <div className="d-flex flex row">
                                <div className="col-6 mb-1">
                                    <label htmlFor="printerPrintsMonth" className="form-label">Piezas impresas este mes</label>
                                    <input id="printerPrintsMonth" type="text" className="input-value-2 w-100" value={data?.userPrintedPrints ?? 0} disabled />
                                </div>
                                <div className="col-6 mb-1">
                                    <label htmlFor="printerPrintsTotal" className="form-label">Piezas impresas en total</label>
                                    <input id="printerPrintsTotal" type="text" className="input-value-2 w-100" value={data?.userTotalPrints ?? 0} disabled />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-7 d-flex flex-column ms-auto">
                    <div className="grey-container-detail mt-2 h-100">
                        <h3 className="title-impact-3 ms-2 mt-2">Piezas impresas</h3>
                        <PrintListDetail id={Number(userId)} typeList={3} />
                    </div>
                </div>
            </div>


        </div>

    );

};
export default UserDetailPage;