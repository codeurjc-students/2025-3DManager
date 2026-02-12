import React, { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { usePopupContext } from "../../context/PopupContext";

import {
    getGroupBasicData,
    updateGroupData,
    leaveGroup,
    deleteGroup,
    kickUserFromGroup,
    transferOwnership,
} from "../../api/groupService";

import type { GroupBasicDataResponse } from "../../models/group/GroupBasicDataResponse";
import { confirmAction } from "./ConfirmAction";

const GroupPopup: React.FC = () => {
    const { user, refreshUser } = useAuth();
    const { showPopup, closePopup } = usePopupContext();
    const [newOwner, setNewOwner] = useState<number>();
    const [data, setData] = useState<GroupBasicDataResponse | null>(null);
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [creationDate, setCreationDate] = useState("");
    const isManager = user?.rolId === "Usuario-Manager";

    useEffect(() => {
        getGroupBasicData().then(response => {
            const group = response.data;
            setData(group!);
            setName(group!.groupName);
            setDescription(group!.groupDescription);
            setCreationDate(new Date(group!.groupDate).toISOString().split("T")[0]);
        });
    }, []);

    const handleSaveGroup = () => {
        confirmAction({
            action: "Guardar cambios",
            service: () => updateGroupData({
                groupName: name,
                groupDescription: description,
                userId: -1
            }),
            successMessage: "Los cambios se han guardado correctamente.",
            errorMessage: "No se pudo guardar el grupo.",
            showPopup,
            reopenGroupPopup
        });
    };

    const handleLeaveGroup = () => {
        confirmAction({
            action: "Abandonar grupo",
            service: () => leaveGroup(),
            successMessage: "Has abandonado el grupo correctamente.",
            errorMessage: "No se pudo abandonar el grupo.",
            showPopup,
            reopenGroupPopup,
            onSuccess: async () => {
                await refreshUser();
                closePopup();
            }
        });
    };

    const handleDeleteGroup = () => {
        confirmAction({
            action: "Eliminar grupo",
            service: () => deleteGroup(),
            successMessage: "El grupo ha sido eliminado correctamente.",
            errorMessage: "No se pudo eliminar el grupo.",
            showPopup,
            reopenGroupPopup,
            onSuccess: async () => {
                await refreshUser();
                closePopup();
            }
        });
    };

    const handleUserExpulsion = (userId: number, userName : string) => {
        confirmAction({
            action: "Expulsar usuario" + userName,
            service: async () => {
                const res = await kickUserFromGroup(userId);
                if (res.data) {
                    const updated = await getGroupBasicData();
                    setData(updated.data!);
                }
                return res;
            },
            successMessage: "El usuario ha sido expulsado correctamente.",
            errorMessage: "No se pudo expulsar al usuario.",
            showPopup,
            reopenGroupPopup
        });
    };


    const handleTransferGroup = () => {
        if (!newOwner) {
            showPopup({
                type: "warning",
                content: "Debes seleccionar un usuario para transferir el control."
            });
            return;
        }

        const selectedUser = data!.groupMembers.find(u => u.userId === newOwner);

        confirmAction({
            action: "Transferir control a " + selectedUser?.userName,
            service: async () => {
                const res = await transferOwnership(newOwner);
                if (res.data) {
                    const updated = await getGroupBasicData();
                    setData(updated.data!);
                }
                return res;
            },
            successMessage: "Has transferido el rol correctamente.",
            errorMessage: "No se ha podido transferir el rol de manager al usuario.",
            showPopup,
            reopenGroupPopup,
            onSuccess: async () => {
                await refreshUser();
                closePopup();
            }
        });
    };


    const reopenGroupPopup = () => {
        showPopup({
            type: "base", width: "80vh", content: (<GroupPopup />)
        });
    };

    if (!data) return <p>Cargando...</p>;

    const isOnlyMember = data.groupMembers.length === 1;


    return (
        <div className="container-fluid">
            <div className="row mt-3">
                <div className="col-12 grey-container p-4">

                    <h2 className="title-impact mb-4">Información del grupo</h2>

                    <div className="white-container p-3 mb-4 text-start">
                        <div className="mb-1 d-flex flex-column">
                            <label className="form-label">Nombre del grupo</label>
                            <input type="text" className="input-value w-100" value={name} disabled={!isManager}
                                onChange={(e) => setName(e.target.value)} />
                        </div>

                        <div className="mb-1 d-flex flex-column">
                            <label className="form-label">Descripción</label>
                            <textarea className="input-value w-100" value={description} disabled={!isManager}
                                onChange={(e) => setDescription(e.target.value)} />
                        </div>
                        <div className="d-flex flex-row mt-2">
                            <div className="mb-1 w-100 me-2 d-flex flex-column">
                                <label className="form-label">Fecha de creación</label>
                                <input type="date" className="input-value w-100" value={creationDate} disabled />
                            </div>
                            <div className="mb-1 w-100 ms-2 d-flex flex-column">
                                <label className="form-label ">Dueño del grupo</label>
                                <input type="text" className="input-value w-100" value={data.groupOwner} disabled/>
                            </div>
                        </div>                     
                    </div>

                    <h4 className="mb-3 text-start">Miembros del grupo</h4>

                    <div className="white-container p-3 h-30">
                        <div className="table-container pt-4">
                            <div className="table-scroll">
                                <table className="table">
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            {isManager && <th>Acción</th>}
                                        </tr>
                                    </thead>
                                    <tbody className="text-start">
                                            {data.groupMembers.map((users) => (
                                                <tr key={users.userId}>
                                                    <td className="w-75">{users.userName}</td>
                                                    {isManager && users.userId !== user!.userId ? (
                                                        <td>
                                                            <button className="button-darkGrey w-100" onClick={() => handleUserExpulsion(users.userId, users.userName)}>
                                                                Expulsar
                                                            </button>
                                                        </td>
                                                    ) :
                                                    <td>
                                                    </td>
                                                }
                                                </tr>
                                            ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    {isManager && !isOnlyMember ? (
                        <div className="white-container h-5 mt-2">
                            <div className="d-flex p-2">
                                <button className="button-red w-20" onClick={handleTransferGroup}>
                                    Transferir control
                                </button>
                                <select id="newOwner" className="input-value w-75 ms-2" value={newOwner ?? ""} onChange={(e) => setNewOwner(Number(e.target.value))}
                                >
                                    <option value="">Seleccione un usuario</option>

                                    {data.groupMembers
                                        .filter(member => member.userId !== user!.userId)
                                        .map(member => (
                                            <option key={member.userId} value={member.userId}>
                                                {member.userName}
                                            </option>
                                        ))}
                                </select>
                            </div>
                        </div>
                    ) : isManager && isOnlyMember ? (
                        <div className="white-container h-5 mt-2 p-3 text-muted">
                            Eres el único miembro del grupo. No puedes transferir el control.
                        </div>
                    ) : null}
                    <div className="d-flex justify-content-between mt-4">

                        {user?.rolId == "Usuario-Base" && (
                            <button className="button-red" onClick={handleLeaveGroup}>
                                Abandonar grupo
                            </button>
                        )}

                        {isManager && (
                            <>
                                <button className="button-yellow w-20" onClick={handleSaveGroup}>
                                    Guardar cambios
                                </button>

                                <button className="button-red w-20" onClick={handleDeleteGroup}>
                                    Eliminar grupo
                                </button>
                            </>
                        )}
                    </div>

                </div>
            </div>
        </div>
    );
};

export default GroupPopup;
