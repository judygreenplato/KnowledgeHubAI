import { Link } from "react-router-dom";

function Navbar() {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div className="container">

        <Link className="navbar-brand" to="/">
          KnowledgeHub AI
        </Link>

        <div className="navbar-nav">

          <Link
            className="nav-link"
            to="/"
          >
            Articles
          </Link>

          <Link
            className="nav-link"
            to="/documents"
          >
            Documents
          </Link>

          <Link
            className="nav-link"
            to="/ask-ai"
          >
            Ask AI
          </Link>

          <Link
            className="nav-link"
            to="/login"
          >
            Login
          </Link>

        </div>
      </div>
    </nav>
  );
}

export default Navbar;