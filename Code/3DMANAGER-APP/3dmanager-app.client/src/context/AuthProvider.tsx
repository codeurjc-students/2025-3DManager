import { useEffect, useState } from "react";
import { AuthContext } from "./AuthContext";
import type { UserObject } from "../models/user/UserObject";
import { GetUserAuth } from "../api/userService";

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<UserObject | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const storedUser = localStorage.getItem("user");

        if (!storedUser) {
            setLoading(false);
            return;
        }

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
                localStorage.removeItem("user");
            })
            .finally(() => setLoading(false));
    }, []);



    const login = (user: UserObject) => {
        setUser(user);
        localStorage.setItem("user", JSON.stringify(user));
    };

    const logout = () => {
        setUser(null);
        localStorage.removeItem("user");
    };

    const refreshUser = async () => {
        try {
            const res = await GetUserAuth();

            const updatedUser = {
                ...user!,
                userId: res.userId,
                groupId: res.groupId,
                rolId: res.rolId,
                groupName: res.groupName
            };

            setUser(updatedUser);
            localStorage.setItem("user", JSON.stringify(updatedUser));

        } catch (err) {
            console.error(err)
            logout();
        }
    };


    return (
        <AuthContext.Provider value={{ user, loading, login, logout, refreshUser }}>
            {children}
        </AuthContext.Provider>
    );
};