import React, { useEffect, useState } from "react";
import { getPrintComments, postPrintComment } from "../api/printService";
import { useAuth } from "../context/AuthContext";
import type { PrintCommentObject } from "../models/print/PrintCommentObject";
import type { PrintCommentRequest } from "../models/print/PrintCommentRequest";

interface PrintCommentsProps {
    id: number;
}

const PrintComments: React.FC<PrintCommentsProps> = ({ id }) => {
    const { user } = useAuth();
    const [comments, setComments] = useState<PrintCommentObject[]>([]);
    const [newComment, setNewComment] = useState("");
    const isLogged =  user?.rolId !== "Usuario-Invitado";
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

     const mockComments: PrintCommentObject[] = [
        
    ];

    return (
        <div className="comments-container ms-3 d-flex flex-column h-100">
            <div className="comments-scroll comments-list h-55">
                {comments.length === 0 ? (
                    <div className="d-flex justify-content-center align-items-center h-100 text-muted small">
                        Sin comentarios en la pieza actual
                    </div>
                ) : (
                    comments.map(c => (
                        <div key={c.commentId} className="comment-item mb-1 p-1">
                            <div className="d-flex justify-content-between">
                                <span className="fw-bold">{c.userName}</span>
                                <span className="small">
                                    {new Date(c.registerDate).toLocaleDateString()}
                                </span>
                            </div>
                            <div className="small">{c.comment}</div>
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
