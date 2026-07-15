import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import ArticlesPage from "./pages/ArticlesPage";

function App() {

    return (
   <>
      <Navbar />

      <div className="container mt-4">

        <Routes>

          <Route
            path="/"
            element={<ArticlesPage />}
          />

          <Route
            path="/documents"
            element={<h2>Documents Page</h2>}
          />

          <Route
            path="/ask-ai"
            element={<h2>Ask AI Page</h2>}
          />

          <Route
            path="/login"
            element={<h2>Login Page</h2>}
          />

        </Routes>

      </div>
    </>
    );
}

export default App;