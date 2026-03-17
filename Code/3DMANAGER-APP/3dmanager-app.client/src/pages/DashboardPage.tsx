import React, { useEffect, useState } from "react";
import DashboardActions from "../components/DashboardActions";
import { useNavigate } from "react-router-dom";
import { getPrinterDashboardList } from "../api/printerService";
import type { PrinterDashboardObject } from "../models/printer/PrinterDashboardObject";
import { DashboardBarChart } from "../components/charts/dashboardChart";
import type { GroupDashboardData } from "../models/group/GroupDashboardData";
import { getGroupDashboardData } from "../api/groupService";

const DashboardPage: React.FC = () => {
    const [printers, setPrinters] = useState<PrinterDashboardObject[]>([]);
    const [data, setData] = useState<GroupDashboardData | null>(null);

    const navigate = useNavigate();

    useEffect(() => {
        getPrinterDashboardList().then(response => {
            setPrinters(response.data ?? []);
        });
        getGroupDashboardData().then(response => {
            setData(response.data ?? null);
        });
    }, []);

     
    const printerHoursChartData = data
        ? data.groupPrinterHours.map(p => ({
            name: p.printerName,
            value: Number(p.printerHours)
        }))
        : [];


    const renderPrinterStatus = (state: number,stateName : string) => {
        switch (state) {
            case 1:
                return <span className="status-badge status-active">{stateName}</span>;
            case 2:
                return <span className="status-badge status-out">{stateName}</span>;
            case 3:
                return <span className="status-badge status-maintenance">{stateName}</span>;
            default:
                return <span className="status-badge status-maintenance">{stateName}</span>;
        }
    };

    return (
        <div id="dashboard" className="d-flex vh-100">
            <div className="grey-container col-6 vh-100 scroll-container">
                <h2 className="title-impact-2 ms-3 mt-4">Impresoras</h2>
                <hr className="m-2"></hr>
                <div>
                    {printers.map((printer) => (
                        <button
                            key={printer.printerId}
                            className="printer-card mb-5 p-3 d-flex flex-row text-start"
                            onClick={() => navigate(`/dashboard/printer/detail/${printer.printerId}`)}
                        >
                            <div className="col-5">
                                <img src={printer.printerImageData?.fileUrl} alt={printer.printerName} className="image-container" />
                            </div>
                            <div className="col-6">
                                <p>{printer.printerName}</p>
                                <p>{printer.printerModel}</p>
                                <p>{printer.printerDescription}</p>
                                {renderPrinterStatus(printer.printerStateId!, printer.printerStateName!)}
                            </div>
                        </button>
                    ))}
                </div>
            </div>
            <div className="white-container col-6 vh-100">
                <h2 className="title-impact-2 ms-3 mt-4">Datos Generales</h2>
                <hr className="m-3"></hr>
                <div className="h-40 d-flex flex-row">
                    <div className="col-4">
                        <p className="title-impact-4 ms-4 mt-5">Horas impresión: {data?.groupTotalHours}</p>
                        <p className="title-impact-4 ms-4 mt-4">Piezas impresas: {data?.groupTotalPrints}</p>
                        <p className="title-impact-4 ms-4 mt-4">Filamento consumido: {data?.groupTotalFilament} m</p>
                        <p className="title-impact-4 ms-4 mt-4">Usuarios activos: {data?.groupUserCount}</p>
                        <p className="title-impact-4 ms-4 mt-4">Filamentos registrados: {data?.groupFilamentCount}</p>
                        <p className="title-impact-4 ms-4 mt-4">Impresoras registradas: {data?.groupPrinterCount}</p>
                    </div>
                    <div className="col-8 charts-scroll">
                        <DashboardBarChart data={printerHoursChartData} height={Math.max((printerHoursChartData.length * 40),150)} />
                    </div>

                </div>
                <div>
                    <DashboardActions />
                </div>  
            </div>
        </div> 

    );

};
export default DashboardPage;

