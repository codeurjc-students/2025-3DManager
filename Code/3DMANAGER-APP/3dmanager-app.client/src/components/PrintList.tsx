import React, { useEffect, useState } from 'react';
import { getPrintList } from '../api/printService';
import { useNavigate } from 'react-router-dom';
import Pagination from './Pagination';
import type { PrintResponse } from '../models/print/PrintResponse';

const PrintList: React.FC = () => {

    const [items, setItems] = useState<PrintResponse[]>([]);
    const navigate = useNavigate();
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        getPrintList(currentPage, pageSize).then(response => {

            const prints = response.data?.prints ?? [];
            const pages = response.data?.totalPages ?? 1;

            if (pages === 0) {
                setItems([]);
                setTotalPages(1);
                setCurrentPage(1);
                return;
            }

            setItems(prints);
            setTotalPages(pages);
        });
    }, [currentPage, pageSize]);


    return (
        <div className="table-container pt-4">
            <div className="table-scroll">
                <table className="table">
                    <thead>
                        <tr>
                            <th className="col-3">Nombre</th>
                            <th>Usuario</th>
                            <th>Fecha impresión</th>
                            <th>Tiempo impresion</th>
                            <th>Filamento consumido</th>
                            <th>Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        {items.map((print) => (
                            <tr key={print.printId}>
                                <td>{print.printName}</td>
                                <td>{print.printUserCreator}</td>
                                <td>{print.printDate.toString()}</td>
                                <td>{print.printTime}</td>
                                <td>{print.printFilamentConsumed}</td>
                                <td>
                                    <button className="button-darkGrey w-75" onClick={() => navigate(`/detail/print/${print.printId}`)}>
                                        Ver detalle
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
            <div className="pagination-fixed">
                <Pagination currentPage={currentPage} totalPages={totalPages} pageSize={pageSize}
                    onPageChange={setCurrentPage} onPageSizeChange={(size) => {
                    setPageSize(size);
                    setCurrentPage(1);
                }}
                />
            </div>
        </div>
    );
};

export default PrintList;