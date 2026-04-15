import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
export default function ErrorPage() {
    const navigate = useNavigate();
    const { user, logout } = useAuth();
    const [params] = useSearchParams();
    const errorId = params.get("code");

    const handleBack = () => {
        if (!user)
            logout();
            navigate("/login");
        if (user!.groupId) {
            navigate("/dashboard");
        } else {
            navigate("/group");
        }
    }

    return (
        <div className="container mt-5">
            <div className="row justify-content-center">
                <div className=" col-12 d-flex flex-row">
                    <div className="col-11"></div>
                    <div className="col-1 mb-1">
                        <button type="button" className="white-container-button d-flex align-items-center" onClick={handleBack}>
                            <span className="dashboard-title pe-3">Volver</span>
                            <svg width="48" height="48" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M18 44V24H30V44M6 18L24 4L42 18V40C42 41.0609 41.5786 42.0783 40.8284 42.8284C40.0783 43.5786 39.0609 44 38 44H10C8.93913 44 7.92172 43.5786 7.17157 42.8284C6.42143 42.0783 6 41.0609 6 40V18Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                            </svg>
                        </button>
                    </div>
                </div>
                <hr></hr>
                <div className="col- text-center">
                    <h1 className="mt-4 title-impact">Ha ocurrido un problema</h1>
                    <p>No se ha podido completar la operación.</p>
                    <p>Es posible que el servidor no esté disponible o que haya ocurrido un error inesperado.</p>

                    {errorId && (
                        <p className="mt-3">
                            Código de error: <strong>{errorId}</strong>
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
}
