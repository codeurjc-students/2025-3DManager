import { createContext, useContext } from "react";

interface NotificationContextType {
    count: number;
    refresh: () => Promise<void>;
}

export const NotificationContext = createContext<NotificationContextType | undefined>(undefined);

export const useNotifications = () => {
    const context = useContext(NotificationContext);

    if (!context) {
        throw new Error("useNotifications must be used within NotificationProvider");
    }

    return context;
};