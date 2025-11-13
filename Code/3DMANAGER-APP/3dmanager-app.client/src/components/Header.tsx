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
                    <div className="row align-items-center">
                        <div className="col d-flex flex-column">
                            <span className="header-name">{user.userName}</span>
                            <span className="header-group">{user.groupName}</span>
                        </div>
                        <div className="col-auto">
                            <svg width="48" height="48" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg" className="logout-icon" onClick={handleLogoutClick}>
                                <path d="M18 42H10C8.93913 42 7.92172 41.5786 7.17157 40.8284C6.42143 40.0783 6 39.0609 6 38V10C6 8.93913 6.42143 7.92172 7.17157 7.17157C7.92172 6.42143 8.93913 6 10 6H18M32 34L42 24M42 24L32 14M42 24H18" stroke="white" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
                            </svg>
                        </div>
                    </div>
                ) : (
                    <span className="text-secondary">No conectado</span>
                )}
            </div>
        </header>
    );
};

export default Header;