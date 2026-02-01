import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { postNewGroup } from "../api/groupService";
import { useAuth } from '../context/AuthContext';
import { usePopupContext } from "../context/PopupContext";

const CreateGroupPage: React.FC = () => {

    const [groupName, setGroupName] = useState("");
    const [groupDescription, setGroupDescription] = useState("");
    const { logout } = useAuth();
    const navigate = useNavigate();
    const { showPopup } = usePopupContext();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); 

        if (!groupName || !groupDescription) {
            showPopup({ type: "warning", title: "Completar formulario", description: "Todos los campos son obligatorios" });
            return;
        }
        
        try {
            let userId = -1; //The real user id is loaded on API with the authentication header.
            
            const response = await postNewGroup({
                groupName,
                groupDescription,
                userId,
            });

            if (response.data) {
                showPopup({ type: "info", title: "Opreación realizada", description: "Grupo creado correctamente. Se va a proceder a hacer un logout para entrar al nuevo grupo" });
                logout();
            } else {
                showPopup({ type: "error", title: "Operación cancelada", description: response.error?.message || "No se pudo crear el grupo"});
            }
        } catch (error) {
            console.error("Error al crear grupo:", error);
            showPopup({ type: "error", title: "Operación cancelada", description: "Ha ocurrido un error en el registro del grupo." });
        }
    };

    return (
        <div className="container-fluid vh-100">         
            <div className="row h-50 mt-5">
                <div className="col-3"></div>
                <div className="grey-container col-6 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-5 mb-5">Crear grupo</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="white-container">
                            <div className="p-3">
                                <div className="mb-3">
                                    <label htmlFor="groupName" className="form-label">Nombre de grupo</label>
                                    <input id="groupName" type="text" className="input-value w-75" value={groupName} placeholder="Nombre grupo"
                                        onChange={(e) => setGroupName(e.target.value)} />
                                </div>

                                <div className="mb-3">
                                    <label htmlFor="groupDescription" className="form-label">Descripción del grupo</label>
                                    <textarea id="groupDescription" className="input-value w-75" value={groupDescription} placeholder="Descripción del grupo"
                                        onChange={(e) => setGroupDescription(e.target.value)} />
                                </div>
                            </div>                           
                        </div>
                        <div className="d-flex justify-content-between  m-5">
                            <button type="submit" className="botton-yellow createUser">Crear grupo</button>
                            <button type="button" className="botton-darkGrey" onClick={() => navigate("/group")}>Cancelar</button>
                        </div>                                      
                    </form>
                </div>
                <div className="col-3"></div>
            </div>
        </div>
        
    );
};
export default CreateGroupPage;
