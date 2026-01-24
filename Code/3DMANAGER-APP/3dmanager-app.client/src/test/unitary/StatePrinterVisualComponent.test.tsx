import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";

const renderPrinterStatus = (state: number, stateName: string) => {
    switch (state) {
        case 1:
            return <span className="status-badge status-active">{stateName}</span>;
        case 2:
            return <span className="status-badge status-out">{stateName}</span>;
        case 3:
            return <span className="status-badge status-maintenance">{stateName}</span>;
        default:
            return <span className="status-badge status-maintenance">{stateName}</span>;
    }
};

describe("renderPrinterStatus", () => {
    it("CorrectActiveSpan", () => {
        render(renderPrinterStatus(1, "Activo"));
        const badge = screen.getByText("Activo");
        expect(badge).toBeInTheDocument();
        expect(badge).toHaveClass("status-badge status-active");
    });

    it("CorrectOutOfServiceSpan", () => {
        render(renderPrinterStatus(2, "Fuera de servicio"));
        const badge = screen.getByText("Fuera de servicio");
        expect(badge).toBeInTheDocument();
        expect(badge).toHaveClass("status-badge status-out");
    });

    it("CorrectMaintenanceSpan", () => {
        render(renderPrinterStatus(3, "Mantenimiento"));
        const badge = screen.getByText("Mantenimiento");
        expect(badge).toBeInTheDocument();
        expect(badge).toHaveClass("status-badge status-maintenance");
    });

    it("CorrectDefaultSpan", () => {
        render(renderPrinterStatus(99, "Desconocido"));
        const badge = screen.getByText("Desconocido");
        expect(badge).toBeInTheDocument();
        expect(badge).toHaveClass("status-badge status-maintenance");
    });
});
