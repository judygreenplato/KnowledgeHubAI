

import { useState } from "react";
import { login } from "../services/authService";

function LoginPage()
{
    const [message, setMessage] =
        useState("");

    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    async function handleLogin()
    {
        const result =
            await login(
                email,
                password);
                 

        localStorage.setItem(
            "token",
            result.accessToken);

            localStorage.setItem(
    "refreshToken",
    result.refreshToken);

        setMessage("Login successful");
      
    }

    return (
        
        <div>
           

            <h1>Login</h1> <br/>

             {
    message &&
    (
        <div className="alert alert-info mt-3">
                                
            {message}
        </div>
    )
}

            <input
                placeholder="Email"
                value={email}
                onChange={e =>
                    setEmail(
                        e.target.value)}
            />

            <br />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={e =>
                    setPassword(
                        e.target.value)}
            />

            <br />

            <button
                onClick={handleLogin}>
                Login
            </button>

        </div>
    );
}

export default LoginPage;