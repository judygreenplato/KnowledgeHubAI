import Navbar from "./components/Navbar";

import { Routes, Route }
from "react-router-dom";

import ArticlesPage
from "./pages/ArticlesPage";

import LoginPage
from "./pages/LoginPage";

import RegisterPage
from "./pages/RegisterPage";

import DocumentsPage
from "./pages/DocumentsPage";

import AskAIPage
from "./pages/AskAIPage";

import DashboardPage
from "./pages/DashboardPage";

function App()
{
    return (
     <>
            <Navbar />
        <Routes>

            <Route
                path="/"
                element={
                    <DashboardPage />
                }
            />

             <Route
                path="/dashboard"
                element={
                    <DashboardPage />
                }
            />

            <Route
                path="/articles"
                element={
                    <ArticlesPage />
                }
            />

            <Route
                path="/documents"
                element={
                    <DocumentsPage />
                }
            />

            <Route
                path="/ask-ai"
                element={
                    <AskAIPage />
                }
            />

            <Route
                path="/login"
                element={
                    <LoginPage />
                }
            />

            <Route
                path="/register"
                element={
                    <RegisterPage />
                }
            />

        </Routes>
        </>
    );
}

export default App;