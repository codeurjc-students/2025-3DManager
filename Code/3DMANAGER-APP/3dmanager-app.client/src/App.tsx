import './App.css'
import React, { useState } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Header from "./components/Header";
import LoginPage from "./pages/LoginPage";
import CreateUserPage from "./pages/CreateUserPage"



const App: React.FC = () => {
    const [usuario, setUsuario] = useState<string | null>(null);

    const handleLogin = (nombre: string) => {
        setUsuario(nombre);
    };

    const handleLogout = () => {
        setUsuario(null);
    };

    return (
        <div className = "main-container">
            <BrowserRouter>
                {/* Common header */}
                <Header user={usuario} onLogout={handleLogout} />

                {/* Contenido principal */}
                <main className = "pages-container">
                    <Routes>
                        <Route path="*" element={<Navigate to="/login" replace />} />
                        <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
                        <Route path="/createUser" element={<CreateUserPage />} />
                    </Routes>
                </main>
            </BrowserRouter>
        </div>
    );
};

export default App;