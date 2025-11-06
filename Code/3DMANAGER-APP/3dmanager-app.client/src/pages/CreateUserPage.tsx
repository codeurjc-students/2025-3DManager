import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { postNewUser } from "../api/userService";

const CreateUserPage: React.FC = () => {

    const [userName, setUserName] = useState("");
    const [userEmail, setUserEmail] = useState("");
    const [userPassword, setUserPassword] = useState("");

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); // Para no recargar la página

        if (!userName || !userEmail || !userPassword) {
            alert("Todos los campos son obligatorios");
            return;
        }
        try {
            // Llamada al servicio
            const response = await postNewUser({
                userName,
                userEmail,
                userPassword,
            });

            if (response.data) {
                alert("Cuenta creada correctamente. Ahora puedes iniciar sesión.");
                navigate("/login");
            } else {
                alert(response.error?.message || "No se pudo crear el usuario.");
            }
        } catch (error) {
            console.error("Error al crear usuario:", error);
            alert("Ha ocurrido un error en el registro del usuario.");
        }
    };

    return (
        <div className="container-fluid vh-100">         
            <div className="row h-75 mt-5">
                <div className="col-3"></div>
                <div className="login-container col-6 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-5 mb-5">Crear cuenta</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label htmlFor="userName" className="form-label">Nombre de usuario</label>
                            <input id="userName" type="text" className="input-value" value={userName} placeholder="Introduce tu nombre"
                                onChange={(e) => setUserName(e.target.value)}/>
                        </div>

                        <div className="mb-3">
                            <label htmlFor="userEmail" className="form-label">Correo electrónico</label>
                            <input id="userEmail" type="email" className="input-value" value={userEmail} placeholder="Introduce tu correo"
                                onChange={(e) => setUserEmail(e.target.value)}/>
                        </div>

                        <div className="mb-3">
                            <label htmlFor="userPassword" className="form-label">Contraseña</label>
                            <input id="userPassword" type="password" className="input-value" value={userPassword} placeholder="Introduce tu contraseña"
                                onChange={(e) => setUserPassword(e.target.value)}/>
                        </div>

                        <div className="d-flex justify-content-between w-50 mt-5">
                            <button type="submit" className="botton-yellow">Crear cuenta</button>
                            <button type="button" className="botton-darkGrey" onClick={() => navigate("/login")}>Volver</button>
                        </div>
                    </form>
                </div>
                <div className="col-3"></div>
            </div>
        </div>
        
    );
};
export default CreateUserPage;
