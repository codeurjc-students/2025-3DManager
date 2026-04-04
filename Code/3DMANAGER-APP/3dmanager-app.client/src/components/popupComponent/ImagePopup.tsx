import React, { useState } from "react";

interface ImagePopupProps {
    title: string;
    isSTLFile: boolean;
    onUpload: (file: File) => Promise<void>;
    onDelete: () => Promise<void>;
    onClose: () => void;
}

const ImagePopup: React.FC<ImagePopupProps> = ({
    title,
    isSTLFile,
    onUpload,
    onDelete,
    onClose
}) => {
    const [file, setFile] = useState<File | null>(null);
    const [loading, setLoading] = useState(false);

    const handleUpload = async () => {
        if (!file) return;
        setLoading(true);
        await onUpload(file);
        setLoading(false);
    };

    const handleDelete = async () => {
        setLoading(true);
        await onDelete();
        setLoading(false);
    };

    return (
        <div className="image-popup-content">
            <h3 className="title-impact-2">{title}</h3>
            {isSTLFile ? (< input type="file" accept=".stl" onChange={(e) => setFile(e.target.files?.[0] || null)} />) :
                (< input type="file" accept="image/*" onChange={(e) => setFile(e.target.files?.[0] || null)} />)}

            <div className="popup-image-buttons mt-3">
                <button className="button-darkGrey col-4" onClick={onClose}>
                    Cancelar
                </button>
                {isSTLFile ?
                    (<button className="button-red " onClick={handleDelete} disabled={loading}>
                        Eliminar STL
                    </button>) :
                    (<button className="button-red " onClick={handleDelete} disabled={loading}>
                        Eliminar imagen 
                    </button>)
                     }

                <button className="button-yellow col-4" onClick={handleUpload} disabled={!file || loading}>
                    Enviar
                </button>
            </div>
        </div>
    );
};

export default ImagePopup;
