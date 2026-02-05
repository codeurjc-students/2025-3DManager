import React, { useEffect, useState } from 'react';
import { getFilamentList } from '../api/filamentService';
import type { FilamentListResponse } from '../models/filament/FilamentListResponse';
import { useNavigate } from 'react-router-dom';
import Pagination from './Pagination';



const FilamentList: React.FC = () => {
    const [items, setItems] = useState<FilamentListResponse[]>([]);
    const navigate = useNavigate();
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    

    useEffect(() => {
        getFilamentList().then(response => {
            setItems(response.data ?? []);
        });
    }, []);

    const totalPages = Math.ceil(items.length / pageSize);
    const paginatedItems = items.slice((currentPage - 1) * pageSize, currentPage * pageSize);

    return (
        <div className="table-container pt-4">
            <div className="table-scroll">
                <table className="table">
                    <thead>
                        <tr>
                            <th className="col-3">Nombre</th>
                            <th>Estado</th>
                            <th>Filamento restante</th>
                            <th>Coste filamento</th>
                            <th>Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        {paginatedItems.map((filament) => (
                            <tr key={filament.filamentId}>
                                <td>{filament.filamentName}</td>
                                <td>{filament.filamentState}</td>
                                <td>{filament.filamentLength}</td>
                                <td>{filament.filamentCost}</td>
                                <td>
                                    <button
                                        className="button-darkGrey w-75"
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
            <div className="pagination-fixed">
                <Pagination currentPage={currentPage} totalPages={totalPages} pageSize={pageSize} onPageChange={setCurrentPage} onPageSizeChange={(size) => {
                    setPageSize(size);
                    setCurrentPage(1); 
                }} />
            </div>
        </div>
    );
};

export default FilamentList;