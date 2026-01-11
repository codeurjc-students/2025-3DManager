import React, { useEffect, useState } from 'react';
import type { UserListResponse } from '../models/user/UserListResponse';
import { getUserInvitationList, postUserInvitation } from '../api/userService';

const UserInvitationList: React.FC = () => {
    const [items, setItems] = useState<UserListResponse[]>([]);

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
                                    onClick={() => postUserInvitation(userList.userId)}>
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