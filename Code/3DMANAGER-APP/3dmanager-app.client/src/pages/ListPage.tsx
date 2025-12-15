import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import FilamentList from "../components/FilamentList";
import PrintList from "../components/PrintList";
import UserList from "../components/UserList";
import UserInvitationList from "../components/UserInvitationList";


const TITLES: Record<string, string> = {
    filaments: "filamentos",
    users: "Usuarios",
    prints: "Piezas",
    invitation: "invitaciones usuario"
};

const ListPage: React.FC = () => {

    const { type } = useParams();
    const navigate = useNavigate();
    const title = TITLES[type ?? ""] ?? "Iventario";

    return (
        <div className="container-fluid vh-100">
            <div className="row h-10">
                <div className="col-10">
                    <h2 className="title-impact-2 mt-3 mb-1">Listado {title}</h2>
                </div>
                <div className="col-2 mt-1 ps-5">
                    <button type="button" className="white-container-button d-flex align-items-center" onClick={() => navigate("/dashboard")}>
                        <span className ="dashboard-title pe-5">Volver</span>
                        <svg width="48" height="48" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M18 44V24H30V44M6 18L24 4L42 18V40C42 41.0609 41.5786 42.0783 40.8284 42.8284C40.0783 43.5786 39.0609 44 38 44H10C8.93913 44 7.92172 43.5786 7.17157 42.8284C6.42143 42.0783 6 41.0609 6 40V18Z" stroke="#1E1E1E" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                    </button>
                </div>
                <hr></hr>
            </div>
            <div className="row h-70 d-flex mt-4">
                <div className="col-1"></div>
                <div className="col-10 grey-container">
                    {type === "filaments" && <FilamentList />}
                    {type === "users" && <UserList />}
                    {type === "prints" && <PrintList />}
                    {type === "invitations" && <UserInvitationList />}
                </div>
                <div className="col-1"></div>
            </div>
            <div className="row h-20"></div>
        </div>

    );
};
export default ListPage;