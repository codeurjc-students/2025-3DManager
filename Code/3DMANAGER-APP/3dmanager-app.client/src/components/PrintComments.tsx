import React, { useEffect, useState } from "react";
import { deletePrintComment, getPrintComments, postPrintComment } from "../api/printService";
import { useAuth } from "../context/AuthContext";
import type { PrintCommentObject } from "../models/print/PrintCommentObject";
import type { PrintCommentRequest } from "../models/print/PrintCommentRequest";
import { usePopupContext } from "../context/PopupContext";
import ConfirmPopup from "../components/popupComponent/ConfirmPopup";
import InfoPopup from "../components/popupComponent/InfoPopup";
import { useNavigate } from "react-router-dom";
interface PrintCommentsProps {
    id: number;
}

const PrintComments: React.FC<PrintCommentsProps> = ({ id }) => {
    const { user } = useAuth();
    const [comments, setComments] = useState<PrintCommentObject[]>([]);
    const [newComment, setNewComment] = useState("");
    const isLogged = user?.rolId !== "Usuario-Invitado";
    const { showPopup, closePopup } = usePopupContext();

    useEffect(() => {
        getPrintComments(id).then(response => {
            setComments(response.data ?? []);
        });
    }, [id]);

    const handleAddComment = async () => {
        if (!newComment.trim()) return;

        const request: PrintCommentRequest = {
            printId: id,
            userId: Number(user?.userId),
            comment: newComment
        };

        const response = await postPrintComment(request);

        if (response.data) {
            const newId = response.data;

            setComments(prev => [
                ...prev,
                {
                    commentId: newId,
                    comment: newComment,
                    userId: Number(user?.userId),
                    userName: user?.userName ?? "Usuario",
                    registerDate: new Date().toISOString()
                }
            ]);

            setNewComment("");
        }
    };

    const handleDelete = (deletedCommentId : number) => {
        showPopup({
            type: "base",
            hideCloseButton: true,
            content: (
                <ConfirmPopup
                    action="Eliminar comentario en impresión"
                    onCancel={() => closePopup()}
                    onConfirm={async () => {
                        const response = await deletePrintComment(deletedCommentId);

                        if (response.data) {
                            const updated = await getPrintComments(id);
                            setComments(updated.data ?? []);

                            showPopup({
                                type: "info",
                                content: (
                                    <InfoPopup
                                        title="Operación realizada"
                                        description="El comentario en la impresión 3d ha sido eliminado correctamente."
                                    />
                                ),
                                onClose: () => closePopup()
                            });
                        } else {
                            showPopup({
                                type: "error",
                                content: (
                                    <InfoPopup
                                        title="Error"
                                        description={response.error?.message || "No se pudo eliminar el comentario de la impresión 3d."}
                                    />
                                ),
                                onClose: () => closePopup()
                            });
                        }
                    }}
                />
            )
        });
    };

    return (
        <div className="comments-container ms-3 d-flex flex-column h-100">
            <div className="comments-scroll comments-list h-55">
                {comments.length === 0 ? (
                    <div className="d-flex justify-content-center align-items-center h-100 text-muted small">
                        Sin comentarios en la pieza actual
                    </div>
                ) : (
                    comments.map(c => (
                        <div key={c.commentId} className="d-flex flex-row">
                            <div className="comment-item mb-1 p-1">
                                <div className="d-flex justify-content-between">
                                    <span className="fw-bold">{c.userName}</span>
                                    <span className="small">
                                        {new Date(c.registerDate).toLocaleDateString()}
                                    </span>
                                </div>
                                <div className="small">{c.comment}</div>
                            </div>
                            {(user?.rolId === "Usuario-Manager" || user?.userId === c.userId) && user.rolId !== 'Usuario-Invitado' ?
                                <button className="delete-comment" onClick={() => handleDelete(c.commentId)} title="Eliminar comentario">
                                    <svg width="32" height="32" viewBox="0 0 32 32" fill="none">
                                        <path d="M4 7.99996H6.66667M6.66667 7.99996H28M6.66667 7.99996L6.66667 26.6666C6.66667 27.3739 6.94762 28.0521 7.44772 28.5522C7.94781 29.0523 8.62609 29.3333 9.33333 29.3333H22.6667C23.3739 29.3333 24.0522 29.0523 24.5523 28.5522C25.0524 28.0521 25.3333 27.3739 25.3333 26.6666V7.99996M10.6667 7.99996V5.33329C10.6667 4.62605 10.9476 3.94777 11.4477 3.44767C11.9478 2.94758 12.6261 2.66663 13.3333 2.66663H18.6667C19.3739 2.66663 20.0522 2.94758 20.5523 3.44767C21.0524 3.94777 21.3333 4.62605 21.3333 5.33329V7.99996M13.3333 14.6666V22.6666M18.6667 14.6666V22.6666"
                                            stroke="#1E1E1E" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" />
                                    </svg>
                                </button> :""}
                        </div>                  
                    ))
                )}
            </div>

            <div className="add-comment mt-1 mb-2 d-flex flex-row h-10">
                
                <textarea
                    className="input-value-5 col-9 me-5"
                    rows={1}
                    placeholder="Escribe un comentario..."
                    value={newComment}
                    maxLength={500}
                    onChange={(e) => setNewComment(e.target.value)}
                    disabled={!isLogged}
                />
                <button className={`col-2" ${isLogged ? "button-yellow" : "button-darkGrey"
                    }`} onClick={handleAddComment}>
                    Añadir comentario
                </button>
            </div>
            <div className=" h-25">
            </div>
        </div>
    );
};

export default PrintComments;
