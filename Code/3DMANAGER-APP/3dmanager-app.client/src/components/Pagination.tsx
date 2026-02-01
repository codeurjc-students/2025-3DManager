import React from "react";

interface PaginationProps {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    onPageChange: (page: number) => void;
    onPageSizeChange: (size: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
    currentPage,
    totalPages,
    pageSize,
    onPageChange,
    onPageSizeChange
}) => {

    return (
        <div className="pagination-container d-flex justify-content-between align-items-center mt-3">

            <div className="d-flex align-items-center">
                <label className="me-2">Elementos por página:</label>
                <select className="form-select form-select-sm w-auto" value={pageSize} onChange={(e) => onPageSizeChange(Number(e.target.value))}>
                    <option value={10}>10</option>
                    <option value={20}>20</option>
                    <option value={50}>50</option>
                </select>
            </div>

            <div className="pagination-buttons btn-group">

                <button className="btn btn-outline-secondary" disabled={currentPage === 1} onClick={() => onPageChange(1)}>
                    {"<<"}
                </button>

                <button className="btn btn-outline-secondary" disabled={currentPage === 1} onClick={() => onPageChange(currentPage - 1)}>
                    {"<"}
                </button>

                <span className="px-3 d-flex align-items-center">
                    Página {currentPage} de {totalPages}
                </span>

                <button className="btn btn-outline-secondary" disabled={currentPage === totalPages} onClick={() => onPageChange(currentPage + 1)}>
                    {">"}
                </button>

                <button className="btn btn-outline-secondary" disabled={currentPage === totalPages} onClick={() => onPageChange(totalPages)}>
                    {">>"}
                </button>

            </div>
        </div>
    );
};

export default Pagination;
