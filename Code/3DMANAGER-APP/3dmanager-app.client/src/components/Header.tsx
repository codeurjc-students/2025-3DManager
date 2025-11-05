import React from 'react';
import { useAuth } from '../context/AuthContext';


const Header: React.FC = () => {
    const { user, logout } = useAuth();

    return (
        <header className="header-container">
            <h1 className="app-title">
                <span className="highlight">3D</span>MANAGER
            </h1>

            <div>
                {user ? (
                    <>
                        <span className="me-3 ">Hola, {user.userName}</span>
                        <button className="btn btn-outline-light btn-sm" onClick={logout}>
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