import { useState } from "react";
import { NotificationContext } from "./NotificationContext";
import { getUnreadNotifications } from "../api/notificationService";

export const NotificationProvider = ({ children }: { children: React.ReactNode }) => {
    const [count, setCount] = useState(0);

    const refresh = async () => {
        const response = await getUnreadNotifications();

        if (response.error || !response.data) return;

        setCount(response.data.length);
    };

    return (
        <NotificationContext.Provider value={{ count, refresh }}>
            {children}
        </NotificationContext.Provider>
    );
};