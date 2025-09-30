import React, { useEffect, useState } from "react";
import { getPrinterList } from "./services/PrinterService/PrinterService";
import { PrinterObject } from "./models/Printer/PrinterObject";

function App() {
  const [printers, setPrinters] = useState<PrinterObject[]>([]);

  useEffect(() => {
    const fetchPrinters = async () => {
      try {
        const data = await getPrinterList();
        setPrinters(data);
      } catch (error) {
        console.error("Error al obtener impresoras", error);
        setPrinters([]);
      }
    };
    fetchPrinters();
  }, []);
  
  return (
  <div>
    {printers && printers.length > 0 ? (
      printers.map(p => <div key={p.PrinterName}>{p.PrinterName}</div>)
    ) : (
      <p>No hay impresoras disponibles.</p>
    )}
  </div>
);
}

export default App;

