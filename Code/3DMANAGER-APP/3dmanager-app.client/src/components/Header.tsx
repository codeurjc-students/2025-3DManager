import React from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from "react-router-dom";
import { usePopupContext } from "../context/PopupContext";
import GroupPopup from './popupComponent/GroupPopup';
import { useNotifications } from "../context/NotificationContext";
import NotificationPopup from './popupComponent/NotificationPopup';

const Header: React.FC = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const { showPopup } = usePopupContext();
    const { count, refresh } = useNotifications();

    const handleNotifications = () => {
        refresh();
        showPopup({
            type: "base",
            width: "500px",
            content: (
                <NotificationPopup onClose={() => {
                    refresh();
                }}/>
            ),
            onClose: () => refresh()
        });
    };


    const handleLogoutClick = () => {
        logout();
        navigate("/login");
    };

    const handleGroup = () => {
        showPopup({
            type: "base", width: "80vh", content: (<GroupPopup/>)
        });
    };

    return (
        <header className="header-container">
            <h1 className="app-title">
                <span className="highlight">3D</span>MANAGER
            </h1>

            <div>
                {user ? (
                    <div className="row align-items-center">
                        <div className="col-auto">
                            <button className="header-notification" onClick={handleNotifications}>
                                <svg width="47" height="46" viewBox="0 0 47 46" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M7.83317 38.3333C6.75609 38.3333 5.83404 37.958 5.06702 37.2073C4.30001 36.4566 3.9165 35.5542 3.9165 34.5V11.5C3.9165 10.4458 4.30001 9.5434 5.06702 8.79271C5.83404 8.04202 6.75609 7.66667 7.83317 7.66667H39.1665C40.2436 7.66667 41.1656 8.04202 41.9326 8.79271C42.6997 9.5434 43.0832 10.4458 43.0832 11.5V34.5C43.0832 35.5542 42.6997 36.4566 41.9326 37.2073C41.1656 37.958 40.2436 38.3333 39.1665 38.3333H7.83317ZM23.4998 24.9167L7.83317 15.3333V34.5H39.1665V15.3333L23.4998 24.9167ZM23.4998 21.0833L39.1665 11.5H7.83317L23.4998 21.0833ZM7.83317 15.3333V11.5V34.5V15.3333Z" fill="white" />
                                </svg>
                                {count > 0 && (
                                    <span className="notif-badge">{count}</span>
                                )}
                            </button>
                        </div>
                        <div className="col d-flex flex-column">
                            <button className="header-name" onClick={() => navigate(`/dashboard/user/detail/${user.userId}`)}>{user.userName}</button>
                            <button className="header-group" onClick={handleGroup}>{user.groupName}</button>
                        </div>
                        <div className="col-auto">
                            <svg width="48" height="48" viewBox="0 0 48 48" fill="none" className="logout-icon" onClick={handleLogoutClick}>
                                <path d="M18 42H10C8.93913 42 7.92172 41.5786 7.17157 40.8284C6.42143 40.0783 6 39.0609 6 38V10C6 8.93913 6.42143 7.92172 7.17157 7.17157C7.92172 6.42143 8.93913 6 10 6H18M32 34L42 24M42 24L32 14M42 24H18" stroke="white" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
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