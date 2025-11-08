import React from "react";
import { useNavigate } from "react-router-dom";

const GroupPage: React.FC = () => {


    const navigate = useNavigate();

    return (
        <div className="container-fluid vh-100">
            <div className="row h-25"></div>
            <div className="row h-40">
                <div className="col-2"></div>
                <div className="grey-container col-8 ps-4 pb-4 d-flex flex-column">
                    <h2 className="title-impact mt-4 mb-2">Grupo</h2>
                    <span > La aplicación se gestiona mediante grupos. Para su uso tendrá que ser invitado a participar en un grupo o crear uno.</span>
                    <div className= "d-flex mt-3 h-50">
                        <div className="col-4 m-3 p-4" >
                            <button type="button" className="botton-yellow createGroup" onClick={() => navigate("/createGroup")}>
                                Crear grupo
                            </button>
                        </div>
                        <div className="col-8 ms-2 me-2">
                            <h4 className="title-impact-2" >Invitaciones</h4>
                            <div className="white-container m-2 w-100 h-100">
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-2"></div>
            </div>
        </div>
        
    );
};
export default GroupPage;
