import './App.css'
import React  from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import { AuthProvider } from "./context/AuthContext";
import { PopupProvider } from "./context/PopupContext";
import { ProtectedRoute } from './context/ProtectedRoute';

import Header from "./components/Header";
import LoginPage from "./pages/LoginPage";
import CreateUserPage from "./pages/CreateUserPage"
import GroupPage from "./pages/GroupPage";
import CreateGroupPage from "./pages/CreateGroupPage";  
import DashboardPage from './pages/DashboardPage';
import ListPage from './pages/ListPage';
import InsertInventoryPage from './pages/InsertInventoryPage';
import CreatePrinterPage from './pages/CreatePrinterPage';
import CreateFilamentPage from './pages/CreateFilamentPage';
import CreatePrint3DPage from './pages/CreatePrint3DPage';
import AuthWatcher from './context/AuthWatcher';



const App: React.FC = () => {

    

    return (
        <AuthProvider>
            <PopupProvider>
                <BrowserRouter>
                    <AuthWatcher />
                    <div className="main-container">
                        <Header />
                        <main className="pages-container">
                            <Routes>
                                <Route path="*" element={<Navigate to="/login" replace />} />
                                <Route path="/login" element={<LoginPage />} />
                                <Route path="/user-create" element={<CreateUserPage />} />
                                <Route path="/group" element={<ProtectedRoute><GroupPage /></ProtectedRoute>} />
                                <Route path="/group-create" element={<ProtectedRoute><CreateGroupPage /></ProtectedRoute>} />
                                <Route path="/dashboard" element={<ProtectedRoute><DashboardPage /></ProtectedRoute>} />
                                <Route path="dashboard/lists/:type" element={<ProtectedRoute><ListPage /></ProtectedRoute>} />
                                <Route path="/dashboard/add" element={<ProtectedRoute><InsertInventoryPage /></ProtectedRoute>} />
                                <Route path="/dashboard/printer-create" element={<ProtectedRoute><CreatePrinterPage /></ProtectedRoute>} />
                                <Route path="/dashboard/filament-create" element={<ProtectedRoute><CreateFilamentPage /></ProtectedRoute>} />
                                <Route path="/dashboard/print-create" element={<ProtectedRoute><CreatePrint3DPage /></ProtectedRoute>} />
                                <Route path="/dashboard/user-invitation/:type" element={<ProtectedRoute><ListPage /></ProtectedRoute>} />
                            </Routes>
                        </main>
                    </div>
                </BrowserRouter>
            </PopupProvider>
        </AuthProvider>
    );
};
export default App;