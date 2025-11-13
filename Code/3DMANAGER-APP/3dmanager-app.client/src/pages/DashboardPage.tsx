import React from "react";

const DashboardPage: React.FC = () => {

    return (
        <div id="dashboard d-flex">
            <div className="grey-container col-6 vh-100">
                <h2 className="title-impact-2 ms-3 mt-4">Impresoras</h2>
                <hr className="m-3"></hr>
            </div>
            <div className="white-container col-6 vh-100">
                <h2 className="title-impact-2 ms-3 mt-4">Datos Generales</h2>
                <hr className ="m-3"></hr>
            </div>
        </div> 
    );

};
export default DashboardPage;
