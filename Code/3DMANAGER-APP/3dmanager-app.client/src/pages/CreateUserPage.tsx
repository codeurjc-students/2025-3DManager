import React from "react";
import { useNavigate } from "react-router-dom";

const CreateUserPage: React.FC = () => {
    const navigate = useNavigate();
    return (
        <div className="container-fluid vh-100">         
            <div className="row h-75 mt-5">
                <div className="col-3"></div>
                <div className="login-container col-6 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-5 mb-5">Crear cuenta</h2>
                    <form>
                        <div className="mb-3">
                            <label htmlFor="nombreUsuario" className="form-label">Nombre de usuario</label>
                            <input id="nombreUsuario" type="text" className="input-value" placeholder="Introduce tu nombre" />
                        </div>

                        <div className="mb-3">
                            <label htmlFor="email" className="form-label">Correo electrónico</label>
                            <input id="email" type="email" className="input-value" placeholder="Introduce tu correo" />
                        </div>

                        <div className="mb-3">
                            <label htmlFor="password" className="form-label">Contraseña</label>
                            <input id="password" type="password" className="input-value" placeholder="Introduce tu contraseña" />
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
