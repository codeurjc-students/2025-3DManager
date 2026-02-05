import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from '../context/AuthContext';
import type { GroupInvitation } from "../models/group/GroupInvitation";
import { getGroupInvitations, postAcceptInvitation } from "../api/groupService";
import { usePopupContext } from "../context/PopupContext";


const GroupPage: React.FC = () => {
    const [items, setItems] = useState<GroupInvitation[]>([]);
    const { logout } = useAuth();
    const { showPopup } = usePopupContext();

    useEffect(() => {
        getGroupInvitations().then(response => {
            setItems(response.data ?? []);
        });
    }, []);

    const reloadInvitations = async () => {
        const response = await getGroupInvitations();
        setItems(response.data ?? []);
    };
    const navigate = useNavigate();

    const handleInvitationResponse = async (groupId: number, accepted: boolean) => {
        try {
            const response = await postAcceptInvitation(groupId, accepted);

            if (response) {
                if (accepted) {
                    showPopup({ type: "info", title: "Operación realizada", description: "Te has unido al grupo. Se va a proceder a hacer un logout para entrar al nuevo grupo" });
                    logout();
                } else {
                    showPopup({ type: "info", title: "Operación realizada", description: "Invitacion rechazada correctamente" });
                    reloadInvitations();
                }
                
            } else {
                showPopup({ type: "error", title: "Operación cancelada", description: "No se ha podido procesar la respuesta a la invitación" });

            }

        } catch (error) {
            console.error("Error procesando la invitación:", error);
            showPopup({ type: "error", title: "Operación cancelada", description: "No se ha podido procesar la respuesta a la invitación" });
        }
    };

    return (
        <div className="container-fluid vh-100">
            <div className="row h-20"></div>
            <div className="row h-60">
                <div className="col-2"></div>
                <div className="grey-container col-8 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-4 mb-2">Grupo</h2>
                    <span > La aplicación se gestiona mediante grupos. Para su uso tendrá que ser invitado a participar en un grupo o crear uno.</span>
                    <div className= "d-flex mt-3 h-50">
                        <div className="col-2 m-3 p-2" >
                            <button type="button" className="button-yellow createGroup" onClick={() => navigate("/group-create")}>
                                Crear grupo
                            </button>
                        </div>
                        <div className="col-10 ms-2 me-2">
                            <h4 className="title-impact-2" >Invitaciones</h4>
                            <div className="white-container m-2 w-90 h-100">
                                <div className="invitation-container">
                                    <table className="table">
                                        <tbody>
                                            {items.map((groupInvitations) => (
                                                <tr key={groupInvitations.groupId}>
                                                    <td className="w-75">El usuario {groupInvitations.userGroupManager} le ha invitado a participar
                                                        en el grupo {groupInvitations.groupName}</td>
                                                    <td>
                                                        <button
                                                            className="button-darkGrey justify-content-end w-100"
                                                            onClick={() => handleInvitationResponse(groupInvitations.groupId, true)}>
                                                            Aceptar
                                                        </button>
                                                    </td>
                                                    <td>
                                                        <button
                                                            className="button-darkGrey justify-content-end w-100"
                                                            onClick={() => handleInvitationResponse(groupInvitations.groupId, false)}>
                                                            Rechazar
                                                        </button>
                                                    </td>
                                                </tr>
                                            ))}
                                        </tbody>                   
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-2"></div>
            </div>
        </div>
        
    );
};
export default GroupPage;


