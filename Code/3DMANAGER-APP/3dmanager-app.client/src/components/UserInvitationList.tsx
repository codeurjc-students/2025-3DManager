import React, { useEffect, useState } from 'react';
import type { UserListResponse } from '../models/user/UserListResponse';
import { getUserInvitationList, postUserInvitation } from '../api/userService';
import Pagination from './Pagination';
import { usePopupContext } from '../context/PopupContext';
import InfoPopup from './popupComponent/InfoPopup';

const UserInvitationList: React.FC = () => {
    const [items, setItems] = useState<UserListResponse[]>([]);
    const [search, setSearch] = useState("");
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const { showPopup } = usePopupContext();

    useEffect(() => {
        const delay = setTimeout(() => {
            getUserInvitationList(search).then(response => { setItems(response.data ?? []); });
        }, 300);  
        return () => clearTimeout(delay);
    }, [search]);

    const totalPages = Math.ceil(items.length / pageSize);
    const paginatedItems = items.slice((currentPage - 1) * pageSize, currentPage * pageSize);


    const loadData = (filter?: string) => {
        getUserInvitationList(filter).then(response => {
            setItems(response.data ?? []);
        });
    };

    useEffect(() => {
        const delay = setTimeout(() => {
            loadData(search);
        }, 300);
        return () => clearTimeout(delay);
    }, [search]);

    const clearSearch = () => {
        setSearch(""); loadData("");
    };

    const handleInvite = async (userId: number) => {
        const response = await postUserInvitation(userId);
        if (response.data) {
            showPopup({
                type: "info", content: (
                    <InfoPopup title="Invitación enviada" description="El usuario ha sido invitado correctamente" />
                )
            });
        } else {
            showPopup({
                type: "error", content: (
                    <InfoPopup title="Error" description={response.error?.message ?? "Ha habido un error enviando la invitación"} />
                )
            });
        }
    };
    return (
        <div className="table-container">
            <div className="table-scroll">
                <div className="mb-1 mt-1 d-flex flex-row">
                    <input type="text" className="form-control" placeholder="Buscar usuario..." value={search}
                        onChange={(e) => setSearch(e.target.value)} />
                    {search.length > 0 && (
                        <button className="button-yellow  ms-2" onClick={clearSearch} >
                            Limpiar
                        </button>
                    )}
                </div>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Nombre</th>                      
                            <th>Invitar</th>
                        </tr>
                    </thead>
                    <tbody>
                        {paginatedItems.map((userList) => (
                            <tr key={userList.userId}>
                                <td className="w-75">{userList.userName}</td>
                                <td>
                                    <button
                                        className="button-darkGrey justify-content-end"
                                        onClick={() => handleInvite(userList.userId)}>
                                        Invitar
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

export default UserInvitationList;