import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import PrintListDetail from "../components/PrintListDetail";
import { useAuth } from "../context/AuthContext";
import { usePopupContext } from "../context/PopupContext";
import InfoPopup from "../components/popupComponent/InfoPopup";
import type { UserDetailObject } from "../models/user/UserDetailObject";
import type { UserUpdateRequest } from "../models/user/UserUpdateRequest";
import { deleteUser, deleteUserImage, getUserDetail, updateUser, updateUserImage } from "../api/userService";
import ImagePopup from "../components/popupComponent/ImagePopup";
import ConfirmPopup from "../components/popupComponent/ConfirmPopup";


const UserDetailPage: React.FC = () => {
    const navigate = useNavigate();
    const { user,logout } = useAuth();
    const { userId } = useParams<{ userId: string }>();
    const [data, setData] = useState<UserDetailObject>();
    const [name, setName] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const isMyUser = user?.userId == Number(userId) && user?.rolId != "Usuario-Invitado";
    const { showPopup, closePopup } = usePopupContext();


    useEffect(() => {
        refreshUser();
    }, [userId]);


    const refreshUser = async () => {
        const response = await getUserDetail(Number(userId));
        const user = response.data;

        if (user) {
            user.userCreateDate = new Date(user.userCreateDate);
            setData(user);
            setName(user.userName || "");
            setEmail(user.userEmail || "");
        }
    };


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
                await refreshUser();
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

    const openImagePopup = () => {
        showPopup({
            type: "base",
            width: "600px",
            hideCloseButton: true,
            content: (
                <ImagePopup
                    title="Actualizar imagen de usuario"
                    isSTLFile={false}
                    onUpload={async (file) => {
                        const response = await updateUserImage(Number(userId), file);

                        if (response.error) {
                            const { message } = response.error;

                            closePopup();
                            await Promise.resolve();

                            showPopup({
                                type: "error",
                                content: (<InfoPopup title="Error" description={message} />),
                                onClose: () => closePopup()
                            });

                            return;
                        }

                        closePopup();
                        await refreshUser();
                        showPopup({
                            type: "info",
                            content: (<InfoPopup title="Imagen actualizada" description="La imagen se ha actualizado correctamente." />),
                            onClose: () => closePopup()
                        });
                    }}
                    onDelete={async () => {
                        const response = await deleteUserImage(Number(userId));

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
                        await refreshUser();
                        showPopup({
                            type: "info",
                            content: (<InfoPopup title="Imagen eliminada" description="La imagen ha sido eliminada." />),
                            onClose: () => 
                                closePopup()
                        });
                    }}
                    onClose={closePopup}
                />
            )
        });
    };

    const handleBack = () => {
        if (user!.groupId) {
            navigate("/dashboard");
        } else {
            navigate("/group");
        }
    }
     
    const handleDelete = () => {
        if (user?.rolId == "Usuario-Manager") {
            showPopup({
                type: "info",
                content: (
                    <InfoPopup title="Operación no compatible" description="Para que puedas eliminar tu usuario no puedes ser el dueño de un grupo." />
                ),
                onClose: () => {
                    closePopup();
                }
            });
        } else {
            showPopup({
                type: "base",
                hideCloseButton: true,
                content: (
                    <ConfirmPopup
                        action="Eliminar tu usuario"
                        onCancel={() => closePopup()}
                        onConfirm={async () => {
                            const response = await deleteUser(user?.userId!);

                            if (response.data) {
                                showPopup({
                                    type: "info",
                                    content: (
                                        <InfoPopup title="Operación realizada" description="Tu usuario ha sido eliminado correctamente." />
                                    ),
                                    onClose: () => {
                                        closePopup();
                                        logout();
                                        navigate("/login");
                                    }
                                });
                            } else {
                                showPopup({
                                    type: "error",
                                    content: (
                                        <InfoPopup title="Error" description={response.error?.message || "No se pudo eliminar tu usuario."} />
                                    ),
                                    onClose: () => closePopup()
                                });
                            }
                        }}
                    />
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
                    <button type="button" className="white-container-button d-flex align-items-center" onClick={handleBack}>
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
                            <input type="text" className="input-value-3 me-5 w-75 input-editable" value={name ?? ""} disabled={!isMyUser}
                                onChange={(e) => setName(e.target.value)}
                            />
                            {isMyUser ? (
                                <div className="d-flex flex-row">
                                    <button className="button-yellow ms-1 me-2" onClick={handleUpdate} title="Guardar cambios">
                                        <svg width="27" height="28" viewBox="0 0 27 28" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M19.125 24.5V15.1667H7.875V24.5M7.875 3.5V9.33333H16.875M21.375 24.5H5.625C5.02826 24.5 4.45597 24.2542 4.03401 23.8166C3.61205 23.379 3.375 22.7855 3.375 22.1667V5.83333C3.375 5.21449 3.61205 4.621 4.03401 4.18342C4.45597 3.74583 5.02826 3.5 5.625 3.5H18L23.625 9.33333V22.1667C23.625 22.7855 23.3879 23.379 22.966 23.8166C22.544 24.2542 21.9717 24.5 21.375 24.5Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </button>
                                    <button className="button-yellow me-2" onClick={openImagePopup} title="Actualizar imagen">
                                        <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M6.66667 28H25.3333C26.8061 28 28 26.8061 28 25.3333V6.66667C28 5.19391 26.8061 4 25.3333 4H6.66667C5.19391 4 4 5.19391 4 6.66667V25.3333C4 26.8061 5.19391 28 6.66667 28ZM6.66667 28L21.3333 13.3333L28 20M13.3333 11.3333C13.3333 12.4379 12.4379 13.3333 11.3333 13.3333C10.2288 13.3333 9.33333 12.4379 9.33333 11.3333C9.33333 10.2288 10.2288 9.33333 11.3333 9.33333C12.4379 9.33333 13.3333 10.2288 13.3333 11.3333Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </button>
                                    <button className="button-red me-2" onClick={handleDelete} title="Eliminar mi usuario">
                                        <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M4 7.99996H6.66667M6.66667 7.99996H28M6.66667 7.99996L6.66667 26.6666C6.66667 27.3739 6.94762 28.0521 7.44772 28.5522C7.94781 29.0523 8.62609 29.3333 9.33333 29.3333H22.6667C23.3739 29.3333 24.0522 29.0523 24.5523 28.5522C25.0524 28.0521 25.3333 27.3739 25.3333 26.6666V7.99996M10.6667 7.99996V5.33329C10.6667 4.62605 10.9476 3.94777 11.4477 3.44767C11.9478 2.94758 12.6261 2.66663 13.3333 2.66663H18.6667C19.3739 2.66663 20.0522 2.94758 20.5523 3.44767C21.0524 3.94777 21.3333 4.62605 21.3333 5.33329V7.99996M13.3333 14.6666V22.6666M18.6667 14.6666V22.6666" stroke="#1E1E1E" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </button>
                                </div>
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
                                    <input id="Email" type="text" className="input-value-5 me-5 w-100 input-editable" value={email ?? ""} disabled={!isMyUser}
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
                    <div className="grey-container-detail mt-2 h-80 ">
                        <h3 className="title-impact-3 ms-2 mt-2">Piezas impresas</h3>
                        <PrintListDetail id={Number(userId)} typeList={3} />
                    </div>
                </div>
            </div>


        </div>

    );

};
export default UserDetailPage;