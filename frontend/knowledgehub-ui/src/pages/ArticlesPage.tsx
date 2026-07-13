import { useEffect, useState } from "react";
import type { Article } from "../models/article";
import { getArticles } from "../services/articleService";

function ArticlesPage() {

    const [articles, setArticles] =
        useState<Article[]>([]);

    const [loading, setLoading] =
        useState(true);

    useEffect(() => {

        loadArticles();

    }, []);

    async function loadArticles() {

        try {

            const result =
                await getArticles();

            setArticles(result);

        }
        catch (error) {

            console.error(error);

        }
        finally {

            setLoading(false);

        }
    }

    if (loading) {

        return <h2>Loading...</h2>;
    }

    return (
        <div>

            <h1>KnowledgeHub Articles</h1>

            {articles.map(article => (

                <div
                    key={article.id}
                    style={{
                        border: "1px solid lightgray",
                        padding: "10px",
                        marginBottom: "10px"
                    }}
                >
                    <h3>{article.title}</h3>

                    <p>{article.summary}</p>

                    <small>
                        {article.createdAtUtc}
                    </small>

                </div>

            ))}

        </div>
    );
}

export default ArticlesPage;