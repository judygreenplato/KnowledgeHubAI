
import api from "../api/axios";

export async function login(
    email: string,
    password: string)
{
    const response =
        await api.post(
            "/Users/login",
            {
                email,
                password
            });

    return response.data;
}