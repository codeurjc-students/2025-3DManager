import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import type { UserListResponse } from '../models/user/UserListResponse';
import { getUserList } from '../api/userService';

const UserList: React.FC = () => {
    
    const { user } = useAuth();
    const [items, setItems] = useState<UserListResponse[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        getUserList(user!.groupId!).then(response => {
            setItems(response.data ?? []);
        });
    }, []);

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
                    {items.map((userList) => (
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
        </div>
    );
};

export default UserList;