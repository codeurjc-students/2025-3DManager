import React from 'react';

type HeaderProps = {
    user: string | null; // si hay usuario logueado, se muestra botón logout
    onLogout: () => void;
};

const Header: React.FC<HeaderProps> = ({ user, onLogout }) => {
    return (
        <header className="header-container">
            <h1 className="app-title">
                <span className="highlight">3D</span>MANAGER
            </h1>

            <div>
                {user ? (
                    <>
                        <span className="me-3">Hola, {user}</span>
                        <button className="btn btn-outline-light btn-sm" onClick={onLogout}>
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