import{ createContext, useContext } from "react";
import type { UserObject } from "../models/user/UserObject";

export type AuthContextType = {
    user: UserObject | null;
    loading: boolean;
    login: (user: UserObject) => void;
    logout: () => void;
    refreshUser: () => Promise<void>;

};

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) throw new Error("useAuth must be used within AuthProvider");
    return context;
};
