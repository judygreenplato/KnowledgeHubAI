import api from "../api/axios";
import type { Article } from "../models/article";

export async function getArticles()
{
    const response =
        await api.get<Article[]>("/articles");

    return response.data;
}