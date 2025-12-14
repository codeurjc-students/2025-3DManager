import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Login, LoginGuest } from "../api/userService";
import { useAuth } from "../context/AuthContext";
import type { LoginResponse } from "../models/user/LoginResponse";

const LoginPage: React.FC = () => {
    const navigate = useNavigate();
    const { login , user } = useAuth();
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (user) {
            if (user.groupId) {
                navigate("/dashboard");
            } else {
                navigate("/group");
            }
        }
    }, [user]);

    const loginAsGuest = async () => {

        setLoading(true);
        try {
            const result = await LoginGuest();

            if (!result || result.error != null) {
                alert(result?.error?.message || "Problema al intentar acceder como invitado");
                return;
            }

            const userLogged = result.data!;
            authLoad(userLogged);

        } catch (err) {
            console.error("Error login como invitado:", err);
            alert("Error conectando con el servidor como invitado");
        } finally {
            setLoading(false);
        }
    }

    const authLoad = async (userLogged: LoginResponse) => {
        login(userLogged.user, userLogged.token);

        if (userLogged.user.groupId) {
            navigate("/dashboard");
        } else {
            navigate("/group");
        }
    }

    const loginUser = async () => {
        setLoading(true);
        try {
            const result = await Login({ userName: userName, userPassword: password });

            if (!result || result.error != null) {
                alert(result?.error?.message || "Problema al intentar acceder");
                return;
            }

            const userLogged = result.data!;
            authLoad(userLogged);
            
        } catch (err) {
            console.error("Error login:", err);
            alert("Error conectando con el servidor");
        } finally {
            setLoading(false);
        }
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!userName || !password) {
            alert("Introduce usuario y contraseña");
            return;
        }
        loginUser();
        
    };

    return (
        <div className="container-fluid vh-100">
            <div className="row h-50 mt-5 mb-5">
                <div className="col-3"></div>
                <div className="grey-container col-6 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-5 mb-5">Inicio sesión</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <span className="form-label">Usuario</span>
                            <input
                                id="userLogin"
                                type="text"
                                className="input-value"
                                placeholder="Introduce tu usuario"
                                value={userName}
                                onChange={(e) => setUserName(e.target.value)}
                            />
                        </div>

                        <div className="mb-3">
                            <span className="form-label">Contraseña</span>
                            <input
                                id="userPass"
                                type="password"
                                className="input-value"
                                placeholder="Introduce tu contraseña"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </div>

                        <div className="d-flex justify-content-between w-50 mt-5">
                            <button type="submit" className="botton-yellow" disabled={loading}>
                                {loading ? "Accediendo..." : "Acceder"}
                            </button>
                            <button type="button" className="botton-darkGrey" onClick={() => loginAsGuest() }>
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
                    <button type="button" className="botton-darkGrey" onClick={() => navigate("/user-create")}>
                        Crear cuenta
                    </button>
                </div>
                <div className="col-3"></div>
            </div>
        </div>
    );
};

export default LoginPage;
