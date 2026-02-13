import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./AuthContext";

const AuthWatcher: React.FC = () => {
    const { user } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!user) return;

        if (user.groupId) {
            navigate("/dashboard");
        } else {
            navigate("/group");
        }
    }, [user]);

    return null;
};

export default AuthWatcher;
