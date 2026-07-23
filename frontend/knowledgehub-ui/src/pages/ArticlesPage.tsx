import { useEffect, useState } from "react";
import type { Article } from "../models/article";
import { getArticles } from "../services/articleService";

function ArticlesPage() {
    const [articles, setArticles] =
        useState<Article[]>([]);

    const [expandedArticleId, setExpandedArticleId] =
        useState<string | null>(null);

    const [loading, setLoading] =
        useState(true);

    const [error, setError] =
        useState("");

    useEffect(() => {
        loadArticles();
    }, []);

    async function loadArticles() {
        try {
            setLoading(true);

            const result =
                await getArticles();

            setArticles(result);

            // Expand the first article
            if (result.length > 0) {
                setExpandedArticleId(
                    result[0].id
                );
            }
        }
        catch (error) {
            console.error(error);

            setError(
                "Unable to load articles."
            );
        }
        finally {
            setLoading(false);
        }
    }

    function toggleArticle(
        articleId: string
    ) {
        if (
            expandedArticleId ===
            articleId
        ) {
            // Collapse if the same
            // article is clicked
            setExpandedArticleId(null);
        }
        else {
            // Expand the clicked article
            // and collapse the previous one
            setExpandedArticleId(
                articleId
            );
        }
    }

    if (loading) {
        return (
            <div className="container mt-5">
                <div className="text-center">
                    <div
                        className="spinner-border"
                        role="status"
                    >
                    </div>

                    <p className="mt-3">
                        Loading articles...
                    </p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="container mt-5">
                <div
                    className="alert alert-danger"
                    role="alert"
                >
                    {error}
                </div>
            </div>
        );
    }

    return (
        <div className="container mt-5">

            <div className="text-center mb-5">

                <h1 className="fw-bold">
                    KnowledgeHub Articles
                </h1>

                <p className="text-muted">
                    Explore articles and
                    learn from the KnowledgeHub
                    community.
                </p>

            </div>

            <div className="row justify-content-center">

                <div className="col-lg-9">

                    {articles.length === 0 ? (

                        <div
                            className=
                            "alert alert-info text-center"
                        >
                            No articles found.
                        </div>

                    ) : (

                        articles.map(article => {

                            const isExpanded =
                                expandedArticleId ===
                                article.id;

                            return (

                                <div
                                    className=
                                    "card shadow-sm mb-4"
                                    key={article.id}
                                >

                                    <div
                                        className=
                                        "card-header bg-white"
                                        role="button"
                                        onClick={() =>
                                            toggleArticle(
                                                article.id
                                            )
                                        }
                                        style={{
                                            cursor:
                                                "pointer"
                                        }}
                                    >

                                        <div
                                            className=
                                            "d-flex justify-content-between align-items-center"
                                        >

                                            <div>

                                                <h4
                                                    className=
                                                    "mb-1"
                                                >
                                                    {
                                                        article.title
                                                    }
                                                </h4>

                                                <small
                                                    className=
                                                    "text-muted"
                                                >
                                                    {new Date(
                                                        article.createdAtUtc
                                                    ).toLocaleDateString()}
                                                </small>

                                            </div>

                                            <span
                                                className=
                                                "fs-4"
                                            >
                                                {
                                                    isExpanded
                                                        ? "−"
                                                        : "+"
                                                }
                                            </span>

                                        </div>

                                    </div>


                                    <div
                                        className=
                                        "card-body"
                                    >

                                        {
                                            isExpanded ? (

                                                <div>

                                                    <p
                                                        className=
                                                        "card-text"
                                                        style={{
                                                            whiteSpace:
                                                                "pre-line",
                                                                textAlign: 
                                                                  "justify"
                                                        }}
                                                    >
                                                        {
                                                            article.content
                                                        }
                                                    </p>

                                                </div>

                                            ) : (

                                                <p
                                                    className=
                                                    "card-text text-muted"
                                                >
                                                    {
                                                        article.summary
                                                    }
                                                </p>

                                            )
                                        }

                                    </div>

                                </div>

                            );

                        })

                    )}

                </div>

            </div>

        </div>
    );
}

export default ArticlesPage;