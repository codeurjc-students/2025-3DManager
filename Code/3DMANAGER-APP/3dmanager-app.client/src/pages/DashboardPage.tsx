import React from "react";
import DashboardActions from "../components/DashboardActions";

const DashboardPage: React.FC = () => {

    return (
        <div id="dashboard" className="d-flex">
            <div className="grey-container col-6 vh-100">
                <h2 className="title-impact-2 ms-3 mt-4">Impresoras</h2>
                <hr className="m-3"></hr>
            </div>
            <div className="white-container col-6 vh-100">
                <h2 className="title-impact-2 ms-3 mt-4">Datos Generales</h2>
                <hr className="m-3"></hr>
                <div className="h-40"></div>
                <div>
                    <DashboardActions />
                </div>
                
            </div>
        </div> 
    );

};
export default DashboardPage;

