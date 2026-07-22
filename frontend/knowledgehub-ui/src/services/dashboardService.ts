import api from "../api/axios";
import type { Dashboard }
    from "../models/dashboard";

export async function getDashboard()
{
    const response =
        await api.get<Dashboard>(
            "/dashboard");

    return response.data;
}