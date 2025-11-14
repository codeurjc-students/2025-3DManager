import './App.css'
import React  from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { ProtectedRoute } from './context/ProtectedRoute';
import Header from "./components/Header";
import LoginPage from "./pages/LoginPage";
import CreateUserPage from "./pages/CreateUserPage"
import GroupPage from "./pages/GroupPage";
import CreateGroupPage from "./pages/CreateGroupPage";  
import DashboardPage from './pages/DashboardPage';


const App: React.FC = () => {
    return (
        <AuthProvider>
            <BrowserRouter>
                <div className="main-container">
                    <Header />

                    <main className="pages-container">
                        <Routes>
                            <Route path="*" element={<Navigate to="/login" replace />} />
                            <Route path="/login" element={<LoginPage />} />
                            <Route path="/createUser" element={<CreateUserPage />} />
                            <Route path="/group" element={<ProtectedRoute><GroupPage /></ProtectedRoute>} />
                            <Route path="/createGroup" element={<ProtectedRoute><CreateGroupPage /></ProtectedRoute>} />
                            <Route path="/dashboard" element={<ProtectedRoute><DashboardPage /></ProtectedRoute>} />
                        </Routes>
                    </main>
                </div>
            </BrowserRouter>
        </AuthProvider>
    );
};
export default App;