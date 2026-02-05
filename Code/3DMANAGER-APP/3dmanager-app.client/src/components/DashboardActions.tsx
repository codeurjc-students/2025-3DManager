import React, { useEffect, useState } from "react";
import { useAuth } from '../context/AuthContext';
import { useNavigate } from "react-router-dom";

const DashboardActions: React.FC = () => {
    const { user } = useAuth();
    const navigate = useNavigate();
    const [ permission, setPermission ] = useState<boolean>(true);

    useEffect(() => {
        if (user!.rolId == "Usuario-Base" || user!.rolId == "Usuario-Manager")
            setPermission(true);
        else setPermission(false);
    }, []);

    return (
        <div>
            <hr className="m-3"></hr>
            <div className="d-flex align-items-center">
                <div className="col-4 d-flex flex-column align-items-center">
                    <button type="button" className="button-darkGrey  dashboard-icon-btn" onClick={() => navigate("/dashboard/lists/filaments")}>
                        <svg width="94" height="91" viewBox="0 0 94 91" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M46.9999 83.4167C68.6311 83.4167 86.1666 66.4408 86.1666 45.5C86.1666 24.5592 68.6311 7.58337 46.9999 7.58337C25.3688 7.58337 7.83325 24.5592 7.83325 45.5C7.83325 66.4408 25.3688 83.4167 46.9999 83.4167Z" stroke="white" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                            <path d="M58.7499 34.125H35.2499V56.875H58.7499V34.125Z" stroke="white" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                    </button>
                    <span className="dashboard-title">Filamentos</span>
                </div>
                <div className="col-4 d-flex flex-column align-items-center">
                    <button type="button" className="button-darkGrey  dashboard-icon-btn" onClick={() => navigate("/dashboard/lists/users")}>
                        <svg width="98" height="97" viewBox="0 0 98 97" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M80.1489 84.8292L80.2405 76.8376C80.2891 72.5985 78.6497 68.5138 75.6831 65.482C72.7164 62.4501 68.6655 60.7195 64.4214 60.6709L32.4168 60.3041C28.1728 60.2554 24.0832 61.8927 21.0479 64.8558C18.0125 67.8188 16.2799 71.8649 16.2314 76.1039L16.1398 84.0956M64.7878 28.7044C64.6866 37.5317 57.4401 44.6055 48.6023 44.5042C39.7645 44.4029 32.682 37.1649 32.7832 28.3376C32.8844 19.5103 40.1309 12.4364 48.9687 12.5377C57.8065 12.639 64.8889 19.8771 64.7878 28.7044Z" stroke="white" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                    </button>
                    <span className="dashboard-title">Usuarios</span>
                </div>
                <div className="col-4 d-flex flex-column align-items-center">
                    <button type="button" className="button-darkGrey dashboard-icon-btn" onClick={() => navigate("/dashboard/lists/prints")}>
                        <svg width="72" height="72" viewBox="0 0 72 72" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M9.81 20.8799L36 36.03L62.19 20.8799M36 66.2399V35.9999M63 47.9999V23.9999C62.9989 22.9478 62.7212 21.9144 62.1946 21.0034C61.6681 20.0925 60.9112 19.336 60 18.8099L39 6.80995C38.0879 6.28334 37.0532 6.0061 36 6.0061C34.9468 6.0061 33.9121 6.28334 33 6.80995L12 18.8099C11.0888 19.336 10.3319 20.0925 9.80538 21.0034C9.27883 21.9144 9.00108 22.9478 9 23.9999V47.9999C9.00108 49.0521 9.27883 50.0855 9.80538 50.9965C10.3319 51.9074 11.0888 52.6639 12 53.1899L33 65.1899C33.9121 65.7166 34.9468 65.9938 36 65.9938C37.0532 65.9938 38.0879 65.7166 39 65.1899L60 53.1899C60.9112 52.6639 61.6681 51.9074 62.1946 50.9965C62.7212 50.0855 62.9989 49.0521 63 47.9999Z" stroke="white" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                    </button>
                    <span className="dashboard-title">Piezas</span>
                </div>           
            </div>
            <div>
                {user!.rolId == "Usuario-Manager" ? (
                    <div className="d-flex align-items-center mt-1 justify-content-between">
                        <button type="button" className="button-yellow dashboard-icon-btn col-5 ms-5" onClick={() => navigate("/dashboard/print-create")}>
                                <span className="dashboard-title pe-5">Subir archivo G-Code</span>
                                <svg width="45" height="42" viewBox="0 0 45 42" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M5.625 26.25V33.25C5.625 34.1783 6.02009 35.0685 6.72335 35.7249C7.42661 36.3813 8.38044 36.75 9.375 36.75L35.625 36.75C36.6196 36.75 37.5734 36.3813 38.2767 35.7249C38.9799 35.0685 39.375 34.1783 39.375 33.25V26.25M13.125 14L22.5 5.25M22.5 5.25L31.875 14M22.5 5.25L22.5 26.25" stroke="#2C2C2C" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                </svg>
                            </button>
                            <button type="button" className="button-yellow dashboard-icon-btn col-5 me-5" onClick={() => navigate("/dashboard/add")}>
                                <span className="dashboard-title pe-5">Añadir</span>
                            <svg width="48" height="48" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M24 10V38M10 24H38" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                            </svg>
                            </button>
                        </div>
                    ) : ( 
                        <div className="d-flex align-items-center justify-content-center mt-1">
                            <button disabled={!permission} type="button" className={`col-11 mt-4 justify-content-center ${!permission ? ".button-darkGrey" : "button-yellow"
                                }`} onClick={() => navigate("/dashboard/print-create")}>
                                <span className="dashboard-title pe-5">Subir archivo G-Code</span>
                                <svg width="45" height="42" viewBox="0 0 45 42" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M5.625 26.25V33.25C5.625 34.1783 6.02009 35.0685 6.72335 35.7249C7.42661 36.3813 8.38044 36.75 9.375 36.75L35.625 36.75C36.6196 36.75 37.5734 36.3813 38.2767 35.7249C38.9799 35.0685 39.375 34.1783 39.375 33.25V26.25M13.125 14L22.5 5.25M22.5 5.25L31.875 14M22.5 5.25L22.5 26.25" stroke="#2C2C2C" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                                </svg>
                            </button>                           
                        </div>                       
                    )}
                </div>
        </div>
    );
};

export default DashboardActions;