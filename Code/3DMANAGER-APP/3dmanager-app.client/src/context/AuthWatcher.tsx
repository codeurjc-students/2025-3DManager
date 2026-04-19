import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./AuthContext";

const AuthWatcher: React.FC = () => {
    const { user, loading } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (loading) return; 
        if (!user) return;

        if (globalThis.location.pathname.startsWith("/error")) return;

        if (user.groupId && globalThis.location.pathname.startsWith("/dashboard")) return;
        if (!user.groupId && globalThis.location.pathname.startsWith("/group")) return;

        if (user.groupId) {
            navigate("/dashboard");
        } else {
            navigate("/group");
        }
    }, [user, loading]);

    return null;
};

export default AuthWatcher;
