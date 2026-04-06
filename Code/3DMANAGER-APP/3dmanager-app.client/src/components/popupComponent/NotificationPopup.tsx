import React, { useEffect, useState } from "react";
import { getUnreadNotifications, markNotificationAsRead } from "../../api/notificationService";
import type { NotificationObject } from "../../models/notifications/NotificationObject";

interface NotificationPopupProps {
    onClose: () => void;
}

const NotificationPopup: React.FC<NotificationPopupProps> = ({ onClose }) => {
    const [notifications, setNotifications] = useState<NotificationObject[]>([]);
    const [loading, setLoading] = useState(false);

    const load = async () => {
        const response = await getUnreadNotifications();
        if (!response.error) {
            setNotifications(response.data!);
        }
    };

    useEffect(() => {
        load();
    }, []);

    const handleMarkAsRead = async (id: number) => {
        setLoading(true);
        const response = await markNotificationAsRead(id);
        setLoading(false);

        if (!response.error) {
            setNotifications((prev) => prev.filter((n) => n.notificationId !== id));
        }
    };


    return (
        <div className="notification-popup-content">
            <h3 className="popup-title">Notificaciones</h3>

            {notifications.length === 0 && (
                <p className="no-notifications">No tienes notificaciones pendientes.</p>
            )}

            <ul className="notification-list">
                {notifications.map((n) => (
                    <li key={n.notificationId} className="notification-item">
                        <div className="notification-message">{n.notificationMessage}</div>
                        <div className="notification-date">
                            {new Date(n.notificationRegisterDate).toLocaleString()}
                        </div>

                        <button className="button-yellow mt-2" disabled={loading} onClick={() => handleMarkAsRead(n.notificationId)}>
                            Marcar como leída
                        </button>
                    </li>
                ))}
            </ul>

        </div>
    );
};

export default NotificationPopup;
