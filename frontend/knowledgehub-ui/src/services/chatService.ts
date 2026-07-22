import api from "../api/axios";

import type { ChatResponse }
    from "../models/chatResponse";

export async function askAI(
    question: string)
{
    const response =
        await api.post<ChatResponse>(
            "/chat",
            {
                question
            });

    return response.data;
}