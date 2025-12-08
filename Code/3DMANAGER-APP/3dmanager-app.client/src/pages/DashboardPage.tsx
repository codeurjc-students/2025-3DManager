import React, { useEffect, useState } from "react";
import DashboardActions from "../components/DashboardActions";
import { useNavigate } from "react-router-dom";
import { getPrinterDahsboardList } from "../api/printerService";
import { useAuth } from "../context/AuthContext";
import type { PrinterDashboardObject } from "../models/printer/PrinterDashboardObject";

const DashboardPage: React.FC = () => {
    const { user } = useAuth();
    const [printers, setPrinters] = useState<PrinterDashboardObject[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        getPrinterDahsboardList(user!.groupId!).then(response => {
            setPrinters(response.data ?? []);
        });
    }, []);

    return (
        <div id="dashboard" className="d-flex vh-100">
            <div className="grey-container col-6 vh-100 scroll-container">
                <h2 className="title-impact-2 ms-3 mt-4">Impresoras</h2>
                <hr className="m-3"></hr>
                <div>
                    {printers.map((printer) => (
                        <button
                            key={printer.printerId}
                            className="printer-card mb-3 p-3 d-flex flex-row text-start"
                            onClick={() => navigate(`/printer/${printer.printerId}`)}
                        >
                            <div className="col-5">
                                
                            </div>
                            <div className="col-6">
                                <p>{printer.printerName}</p>
                                <p>{printer.printerModel}</p>
                                <p>{printer.printerDescription}</p>
                            </div>
                        </button>
                    ))}
                </div>
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

