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
import CreatePrinterPage from './pages/CreatePrinterPage';
import CreateFilamentPage from './pages/CreateFilamentPage';
import CreatePrint3DPage from './pages/CreatePrint3DPage';
import AuthWatcher from './context/AuthWatcher';
import PrinterDetailPage from './pages/PrinterDetailsPage';
import UserDetailPage from './pages/UserDetailPage';
import FilamentDetailPage from './pages/FilamentDetailsPage';
import PrintDetailPage from './pages/PrintDetailsPage';
import ErrorPage from './pages/ErrorPage';
import { NotificationProvider } from './context/NotificationContext';



const App: React.FC = () => {
    return (
        <AuthProvider>
            <BrowserRouter>
                <PopupProvider>
                    <NotificationProvider>
                        <AuthWatcher />
                        <div className="main-container">
                            <Header />
                            <main className="pages-container">
                                <Routes>
                                    <Route path="/login" element={<LoginPage />} />
                                    <Route path="/user-create" element={<CreateUserPage />} />
                                    <Route path="/group" element={<ProtectedRoute><GroupPage /></ProtectedRoute>} />
                                    <Route path="/group-create" element={<ProtectedRoute><CreateGroupPage /></ProtectedRoute>} />
                                    <Route path="/dashboard" element={<ProtectedRoute><DashboardPage /></ProtectedRoute>} />
                                    <Route path="dashboard/lists/:type" element={<ProtectedRoute><ListPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/printer-create" element={<ProtectedRoute><CreatePrinterPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/filament-create" element={<ProtectedRoute><CreateFilamentPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/print-create" element={<ProtectedRoute><CreatePrint3DPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/user-invitation/:type" element={<ProtectedRoute><ListPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/printer/detail/:printerId" element={<ProtectedRoute><PrinterDetailPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/user/detail/:userId" element={<ProtectedRoute><UserDetailPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/filament/detail/:filamentId" element={<ProtectedRoute><FilamentDetailPage /></ProtectedRoute>} />
                                    <Route path="/dashboard/print/detail/:printId" element={<ProtectedRoute><PrintDetailPage /></ProtectedRoute>} />
                                    <Route path="/error" element={<ErrorPage />} />
                                    <Route path="*" element={<Navigate to="/login" replace />} /> 
                                </Routes>
                            </main>
                        </div>
                    </NotificationProvider>
                </PopupProvider>
            </BrowserRouter>
        </AuthProvider>
    );
};
export default App;