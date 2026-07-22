import { useEffect, useState } from "react";
import  type { Article } from "../models/article";
import { getArticles } from "../services/articleService";

function ArticlesPage()
{
    const [articles, setArticles] =
        useState<Article[]>([]);

    useEffect(() =>
    {
        loadArticles();
    }, []);

    async function loadArticles()
    {
        const result =
            await getArticles();

        setArticles(result);
    }

    return (
        <div>
            <h1>Articles</h1>

            {articles.map(article => (
                <div key={article.id}>
                    <h3>{article.title}</h3>

                    <p>{article.summary}</p>

                    <hr />
                </div>
            ))}
        </div>
    );
}

export default ArticlesPage;