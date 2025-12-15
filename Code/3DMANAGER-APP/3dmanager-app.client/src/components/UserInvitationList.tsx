import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import type { UserListResponse } from '../models/user/UserListResponse';
import { getUserInvitationList, postUserInvitation } from '../api/userService';

const UserInvitationList: React.FC = () => {
    const { user } = useAuth();
    const [items, setItems] = useState<UserListResponse[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        getUserInvitationList().then(response => {
            setItems(response.data ?? []);
        });
    }, []);

    return (
        <div className="table-container">
            <table className="table">
                <thead>
                    <tr>
                        <th>Nombre</th>                      
                        <th>Invitar</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map((userList) => (
                        <tr key={userList.userId}>
                            <td className="w-75">{userList.userName}</td>
                            <td>
                                <button
                                    className="botton-darkGrey justify-content-end"
                                    onClick={() => postUserInvitation(user!.groupId!, userList.userId)}>
                                    Invitar
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default UserInvitationList;