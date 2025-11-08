import React from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from "react-router-dom";

const Header: React.FC = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogoutClick = () => {
        logout();
        navigate("/login");
    };

    return (
        <header className="header-container">
            <h1 className="app-title">
                <span className="highlight">3D</span>MANAGER
            </h1>

            <div>
                {user ? (
                    <>
                        <span className="me-3 ">Hola, {user.userName}</span>
                        <button className="btn btn-outline-light btn-sm" onClick={handleLogoutClick}>
                            Logout
                        </button>
                    </>
                ) : (
                    <span className="text-secondary">No conectado</span>
                )}
            </div>
        </header>
    );
};

export default Header;