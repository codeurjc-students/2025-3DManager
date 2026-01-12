import React from "react";
import { useNavigate } from "react-router-dom";

const InsertInventoryPage: React.FC = () => {


    const navigate = useNavigate();

    return (
        <div className="container-fluid vh-100">
            <div className="row h-25 d-flex justify-content-end">
                <div className="col-2 mt-1 ps-5">
                    <button type="button" className="white-container-button d-flex align-items-center" onClick={() => navigate("/dashboard")}>
                        <span className="dashboard-title pe-5">Volver</span>
                        <svg width="48" height="48" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M18 44V24H30V44M6 18L24 4L42 18V40C42 41.0609 41.5786 42.0783 40.8284 42.8284C40.0783 43.5786 39.0609 44 38 44H10C8.93913 44 7.92172 43.5786 7.17157 42.8284C6.42143 42.0783 6 41.0609 6 40V18Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                    </button>
                </div>
            </div>
            <div className="row h-40 d-flex justify-content-center">
                <div className="col-10 ms-5 d-flex flex-row justify-content-between">
                    <button type="button" className="botton-low-yellow col-3 ms-5 d-flex flex-column align-items-center justify-content-center" onClick={() => navigate("/dashboard/printer-create")}>
                        <svg width="93" height="88" viewBox="0 0 93 88" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M46.5 18.3333V69.6666M19.375 43.9999H73.625" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                        <span className="dashboard-title mt-4">Agregar impresora</span>
                    </button>
                    <button type="button" className="botton-low-yellow col-3 ms-5 d-flex flex-column align-items-center justify-content-center" onClick={() => navigate("/dashboard/filament-create")}>
                        <svg width="93" height="88" viewBox="0 0 93 88" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M46.5 18.3333V69.6666M19.375 43.9999H73.625" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                        <span className="dashboard-title mt-4">Agregar filamento</span>
                    </button>
                    <button type="button" className="botton-low-yellow col-3 ms-5 d-flex flex-column align-items-center justify-content-center" onClick={() => navigate("/dashboard/user-invitation/invitations")}>
                        <svg width="71" height="88" viewBox="0 0 71 68" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M47.3332 59.5V53.8333C47.3332 50.8275 46.0865 47.9449 43.8673 45.8195C41.6482 43.694 38.6383 42.5 35.4999 42.5H14.7916C11.6532 42.5 8.64333 43.694 6.42415 45.8195C4.20497 47.9449 2.95825 50.8275 2.95825 53.8333V59.5M59.1666 22.6667V39.6667M68.0416 31.1667H50.2916M36.9791 19.8333C36.9791 26.0926 31.6811 31.1667 25.1457 31.1667C18.6104 31.1667 13.3124 26.0926 13.3124 19.8333C13.3124 13.5741 18.6104 8.5 25.1457 8.5C31.6811 8.5 36.9791 13.5741 36.9791 19.8333Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                        <span className="dashboard-title mt-4">Agregar usuario al grupo</span>
                    </button>
                </div>
            </div>
        </div>
        
    );
};
export default InsertInventoryPage;
