import { createContext, useContext, useState, useEffect } from "react";
import { getUnreadNotifications } from "../api/notificationService";

interface NotificationContextType {
    count: number;
    refresh: () => Promise<void>;
}

const NotificationContext = createContext<NotificationContextType>({
    count: 0,
    refresh: async () => { }
});

export const useNotifications = () => useContext(NotificationContext);

export const NotificationProvider = ({ children }: { children: React.ReactNode }) => {
    const [count, setCount] = useState(0);

    const refresh = async () => {
        const response = await getUnreadNotifications();
        if (!response.error) {
            setCount(response.data!.length);
        }
    };

    return (
        <NotificationContext.Provider value={{ count, refresh }}>
            {children}
        </NotificationContext.Provider>
    );
};
