import React, { createContext, useState, useEffect, useContext } from "react";
import type { UserObject } from "../models/user/UserObject";
import { GetUserAuth } from "../api/userService";

type AuthContextType = {
    user: UserObject | null;
    token: string | null;
    loading: boolean;
    login: (user: UserObject, token: string) => void;
    logout: () => void;
    refreshUser: () => Promise<void>;

};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<UserObject | null>(null);
    const [token, setToken] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);  

    useEffect(() => {
        const storedToken = localStorage.getItem("token");
        const storedUser = localStorage.getItem("user");

        if (!storedToken || !storedUser) {
            setLoading(false);
            return;
        }

        setToken(storedToken);
        setUser(JSON.parse(storedUser));

        GetUserAuth()
            .then(response => {
                const updatedUser = {
                    ...JSON.parse(storedUser),
                    userId: response.userId,
                    groupId: response.groupId,
                    rolId: response.rolId
                };

                setUser(updatedUser);
                localStorage.setItem("user", JSON.stringify(updatedUser));
            })
            .catch(() => {
                setUser(null);
                setToken(null);
                localStorage.removeItem("user");
                localStorage.removeItem("token");
            })
            .finally(() => setLoading(false));
    }, []);



    const login = (user: UserObject, token: string) => {
        setUser(user);
        setToken(token);
        localStorage.setItem("user", JSON.stringify(user));
        localStorage.setItem("token", token);
    };

    const logout = () => {
        setUser(null);
        setToken(null);
        localStorage.removeItem("user");
        localStorage.removeItem("token");
    };

    const refreshUser = async () => {
        try {
            const res = await GetUserAuth();

            const updatedUser = {
                ...user!,
                userId: res.userId,
                groupId: res.groupId,
                rolId: res.rolId
            };

            setUser(updatedUser);
            localStorage.setItem("user", JSON.stringify(updatedUser));

        } catch {
            logout();
        }
    };


    return (
        <AuthContext.Provider value={{ user, token,loading, login, logout , refreshUser }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) throw new Error("useAuth must be used within AuthProvider");
    return context;
};
