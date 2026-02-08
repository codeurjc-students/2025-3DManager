// components/popupContents/GroupPopup.tsx
import React, { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { usePopupContext } from "../../context/PopupContext";
import ConfirmPopup from "./ConfirmPopup";

import {
    getGroupBasicData,
    updateGroupData,
    leaveGroup,
    deleteGroup,
    kickUserFromGroup,
    //transferGroupOwnership
} from "../../api/groupService";

import type { GroupBasicDataResponse } from "../../models/group/GroupBasicDataResponse";

const GroupPopup: React.FC = () => {
    const { user } = useAuth();
    const { showPopup, closePopup } = usePopupContext();

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

    if (!data) return <p>Cargando...</p>;


    const handleSaveGroup = async () => {
        await updateGroupData({
            groupName: name,
            groupDescription: description,
            userId: -1
        });
        closePopup();
    };

    const handleLeaveGroup = () => {
        showPopup({
            content: (
                <ConfirmPopup
                    action="Abandonar grupo"
                    onConfirm={async () => {
                        await leaveGroup();
                        closePopup();
                    }}
                />
            )
        });
    };

    const handleDeleteGroup = () => {
        showPopup({
            content: (
                <ConfirmPopup
                    action="Eliminar grupo"
                    onConfirm={async () => {
                        await deleteGroup();
                        closePopup();
                    }}
                />
            )
        });
    };

    const handleUserExpulsion = (userId: number) => {
        showPopup({
            content: (
                <ConfirmPopup
                    action="Expulsar usuario"
                    onConfirm={async () => {
                        await kickUserFromGroup(userId);
                        const updated = await getGroupBasicData();
                        setData(updated.data!);
                        closePopup();
                    }}
                />
            )
        });
    };

    const handleTransferGroup = () => {
        console.log("Transferir control");
    };

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
                                            {data.groupMembers.map((m) => (
                                                <tr key={m.userId}>
                                                    <td className="w-75">{m.userName}</td>
                                                    {isManager && m.userId !== user!.userId ? (
                                                        <td>
                                                            <button className="button-darkGrey w-100"  onClick={() => handleUserExpulsion(m.userId)}>
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

                    <div className="d-flex justify-content-between mt-4">

                        {user?.rolId == "usuario-base" && (
                            <button className="button-red" onClick={handleLeaveGroup}>
                                Abandonar grupo
                            </button>
                        )}

                        {isManager && (
                            <>
                                <button className="button-yellow w-20" onClick={handleSaveGroup}>
                                    Guardar cambios
                                </button>

                                <button className="button-red w-20" onClick={handleTransferGroup}>
                                    Transferir control
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
