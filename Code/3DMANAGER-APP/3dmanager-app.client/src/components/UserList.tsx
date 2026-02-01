import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { UserListResponse } from '../models/user/UserListResponse';
import { getUserList } from '../api/userService';
import Pagination from './Pagination';

const UserList: React.FC = () => {
    
    const [items, setItems] = useState<UserListResponse[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const navigate = useNavigate();

    useEffect(() => {
        getUserList().then(response => {
            setItems(response.data ?? []);
        });
    }, []);

    const totalPages = Math.ceil(items.length / pageSize);
    const paginatedItems = items.slice((currentPage - 1) * pageSize, currentPage * pageSize);

    return (
        <div className="table-container">
            <table className="table">
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Horas mes actual</th>
                        <th>Piezas mes actual</th>
                        <th>Detalle</th>
                    </tr>
                </thead>
                <tbody>
                    {paginatedItems.map((userList) => (
                        <tr key={userList.userId}>
                            <td>{userList.userName}</td>
                            <td>{userList.userHours}</td>
                            <td>{userList.userNumberPrints}</td>
                            <td>
                                <button
                                    className="botton-darkGrey w-75"
                                    onClick={() => navigate(`/detail/user/${userList.userId}`)}
                                >
                                    Ver detalle
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <Pagination currentPage={currentPage} totalPages={totalPages} pageSize={pageSize} onPageChange={setCurrentPage} onPageSizeChange={(size) => {
                setPageSize(size);
                setCurrentPage(1); 
            }}
            />
        </div>
    );
};

export default UserList;