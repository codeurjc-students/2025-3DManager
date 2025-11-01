import React from "react";
import { useNavigate } from "react-router-dom";

type LoginPageProps = {
    onLogin: (nombre: string) => void;
};


const LoginPage: React.FC<LoginPageProps> = ({ onLogin }) => {
    const handleLogin = () => {
        // simulamos login con un usuario ficticio
        onLogin("UserName12345");
    };
    const navigate = useNavigate();

    return (
        <div className="container-fluid vh-100">

          <div className="row h-50 mt-5 mb-5">
            <div className="col-3"></div>
                <div className="login-container col-6 ps-4 pb-4 d-flex flex-column">
                <h2 className="title-impact mt-5 mb-5">Inicio sesión</h2>
                <form>
                    <div className="mb-3">
                        <span className="form-label">Usuario</span>
                        <input id="userLogin" type="text" className="input-value" placeholder="Introduce tu usuario" />
                    </div>

                    <div className="mb-3">
                        <span className="form-label">Contraseña</span>
                        <input id="userPass" type="password" className="input-value" placeholder="Introduce tu contraseña" />
                    </div>

                    <div className="d-flex justify-content-between w-50 mt-5">
                            <button type="submit" className="botton-yellow">
                            Acceder
                        </button>
                            <button type="button" className="botton-darkGrey">
                            Acceder como invitado
                        </button>
                    </div>
                </form>
            </div>
            <div className="col-3"></div>
          </div>

          <div className="row mt-5">
                <div className="col-3"></div>
                <div className="col-6 mt-2 d-flex flex-column">
                    <button type="button" className="botton-darkGrey" onClick={() => navigate("/createUser")}>
                        Crear cuenta
                    </button>
                </div>
                <div className="col-3"></div>
          </div>
        </div>
    );
};

export default LoginPage;
