import React, { useEffect, useState } from "react";
import { getPrinters } from "../../services/PrinterService/PrinterService";
import { PrinterObject } from "../../api/generated/apiClient";

const PrinterList = () => {
  const [printers, setPrinters] = useState<PrinterObject[]>([]);

 useEffect(() => {
  const fetchPrinters = async () => {
    try {
      const response = await getPrinters();
      // response.data contiene la lista real de impresoras
      setPrinters(response.data ?? []);
    } catch (err) {
      console.error("Error cargando impresoras:", err);
      setPrinters([]); // opcional: limpiar la lista en caso de error
    }
  };
  console.log(printers);
  fetchPrinters();
}, []);

  return (
    <ul>
      {printers.length > 0 ? (
        printers.map(p => <li key={p.printerName}>{p.printerName}</li>)
      ) : (
        <li>No hay impresoras disponibles</li>
      )}
    </ul>
  );
};

export default PrinterList;

