import React, { useEffect, useState } from "react";
import { PrinterObject } from "../../models/Printer/PrinterObject";
import { getPrinterList } from "../../services/PrinterService/PrinterService";

export const PrinterList: React.FC = () => {
  const [printers, setPrinters] = useState<PrinterObject[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const data = await getPrinterList();
        if (mounted) setPrinters(data);
      } catch (err: any) {
        console.error("Error cargando impresoras:", err);
        setError(err?.message || "Error al obtener impresoras");
      } finally {
        if (mounted) setLoading(false);
      }
    })();
    return () => { mounted = false; };
  }, []);

  if (loading) return <div>Cargando impresoras...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h2>Lista de impresoras</h2>
      <ul>
        {printers.map(p => (
          <li key={p.PrinterName}>{p.PrinterName}</li>
        ))}
      </ul>
    </div>
  );
};
