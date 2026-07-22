import { Link, useNavigate } from "react-router-dom";

function Navbar() {

    const navigate = useNavigate();

    const token =
        localStorage.getItem("token");

    function logout() {

        localStorage.removeItem("token");

        localStorage.removeItem("refreshToken");

        navigate("/login");
    }

    return (

        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">

            <div className="container">

                <Link
                    className="navbar-brand"
                    to="/">
                    KnowledgeHub
                </Link>

                <div className="collapse navbar-collapse">

                    <ul className="navbar-nav me-auto">

                        <li className="nav-item">

                            <Link
                                className="nav-link"
                                to="/articles">
                                Articles
                            </Link>

                        </li>

                        <li className="nav-item">

                            <Link
                                className="nav-link"
                                to="/documents">
                                Documents
                            </Link>

                        </li>

                        <li className="nav-item">

                            <Link
                                className="nav-link"
                                to="/ask-ai">
                                Ask AI
                            </Link>

                        </li>

                        <li className="nav-item">

                            <Link
                                className="nav-link"
                                to="/dashboard">
                                Dashboard
                            </Link>

                        </li>

                    </ul>

                    {

                        token ?

                            <button
                                className="btn btn-outline-light"
                                onClick={logout}>

                                Logout

                            </button>

                            :

                           <div className="d-flex gap-2">

    <Link
        className="btn btn-success"
        to="/login">

        Login

    </Link>

    <Link
        className="btn btn-outline-info"
        to="/register">

        Register

    </Link>

</div>

                    }

                </div>

            </div>

        </nav>

    );
}

export default Navbar;