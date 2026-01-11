import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import type { GroupInvitation } from "../models/group/GroupInvitation";
import { getGroupInvitations, postAcceptInvitation } from "../api/groupService";


const GroupPage: React.FC = () => {
    const [items, setItems] = useState<GroupInvitation[]>([]);

    useEffect(() => {
        getGroupInvitations().then(response => {
            setItems(response.data ?? []);
        });
    }, []);

    const navigate = useNavigate();

    return (
        <div className="container-fluid vh-100">
            <div className="row h-25"></div>
            <div className="row h-40">
                <div className="col-2"></div>
                <div className="grey-container col-8 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-4 mb-2">Grupo</h2>
                    <span > La aplicación se gestiona mediante grupos. Para su uso tendrá que ser invitado a participar en un grupo o crear uno.</span>
                    <div className= "d-flex mt-3 h-50">
                        <div className="col-4 m-3 p-4" >
                            <button type="button" className="botton-yellow createGroup" onClick={() => navigate("/createGroups")}>
                                Crear grupo
                            </button>
                        </div>
                        <div className="col-8 ms-2 me-2">
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
                                                            className="botton-darkGrey justify-content-end w-100"
                                                            onClick={() => postAcceptInvitation(groupInvitations.groupId)}>
                                                            Aceptar invitación
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


