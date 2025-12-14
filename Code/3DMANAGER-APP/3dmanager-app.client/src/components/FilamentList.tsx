import React, { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { getFilamentList } from '../api/filamentService';
import type { FilamentListResponse } from '../models/filament/FilamentListResponse';
import { useNavigate } from 'react-router-dom';



const FilamentList: React.FC = () => {
    const { user } = useAuth();
    const [items, setItems] = useState<FilamentListResponse[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        getFilamentList(user!.groupId!).then(response => {
            setItems(response.data ?? []);
        });
    }, []);

    return (
        <div className="table-container ">
            <table className="table">
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Estado</th>
                        <th>Filamento restante</th>
                        <th>Coste filamento</th>
                        <th>Detalle</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map((filament) => (
                        <tr key={filament.filamentId}>
                            <td>{filament.filamentName}</td>
                            <td>{filament.filamentState}</td>
                            <td>{filament.filamentLength}</td>
                            <td>{filament.filamentCost}</td>
                            <td>
                                <button
                                    className="botton-darkGrey w-75"
                                    onClick={() => navigate(`/detail/filament/${filament.filamentId}`)}
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

export default FilamentList;