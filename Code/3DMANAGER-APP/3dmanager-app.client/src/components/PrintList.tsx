import React, { useEffect, useState } from 'react';
import type { PrintListResponse } from '../models/print/PrintListResponse';
import { getPrintList } from '../api/printService';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const PrintList: React.FC = () => {
    

    const { user } = useAuth();
    const [items, setItems] = useState<PrintListResponse[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        getPrintList(user!.groupId!).then(response => {
            setItems(response.data ?? []);
        });
    }, []);

    return (
        <div className="table-container">
            <table className="table">
                <thead>
                    <tr>
                        <th>Nombre</th>
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
                            <td>{print.printDate.toDateString()}</td>
                            <td>{print.printTime}</td>
                            <td>{print.printFilamentConsumed}</td>
                            <td>
                                <button
                                    className="botton-darkGrey w-75"
                                    onClick={() => navigate(`/detail/print/${print.printId}`)}
                                >
                                    Ver detalle
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default PrintList;