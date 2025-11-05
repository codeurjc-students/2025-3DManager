import './App.css'
import React from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Header from "./components/Header";
import LoginPage from "./pages/LoginPage";
import CreateUserPage from "./pages/CreateUserPage"
import { AuthProvider } from "./context/AuthContext";


const App: React.FC = () => {
    return (
        <div className="main-container">
            <AuthProvider>
                <BrowserRouter>
                    {/* Common header */}
                    <Header />

                    {/* Contenido principal */}
                    <main className = "pages-container">
                        <Routes>
                            <Route path="*" element={<Navigate to="/login" replace />} />
                            <Route path="/login" element={<LoginPage />} />
                            <Route path="/createUser" element={<CreateUserPage />} />
                        </Routes>
                    </main>
                </BrowserRouter>
            </AuthProvider>
        </div>
    );
};

export default App;