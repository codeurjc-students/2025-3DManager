import React from "react";
import PrinterList from "./components/printer/PrinterList";
// Más adelante: import UserList, CoilList, etc.

function App() {
  return (
    <div>
      <h1>3D Manager Dashboard</h1>
      <section>
        <h2>Impresoras</h2>
        <PrinterList />
      </section>
      
    </div>
  );
}

export default App;


