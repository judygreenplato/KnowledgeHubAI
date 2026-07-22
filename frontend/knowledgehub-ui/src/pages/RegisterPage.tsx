

import { useState } from "react";
import api from "../api/axios";

function RegisterPage()
{
    const [message, setMessage] =
        useState("");

    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    async function registerUser()
    {
        await api.post(
            "/Users/register",
            {
                email,
                password
            });

         setMessage(" Registered successfully");
    }

    return (
        <div>

            <h1>Register</h1> <br/>


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
                onClick={
                    registerUser}>
                Register
            </button>

        </div>
    );
}

export default RegisterPage;