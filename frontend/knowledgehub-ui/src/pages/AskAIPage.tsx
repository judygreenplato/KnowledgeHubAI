import { useState }
    from "react";

import { askAI }
    from "../services/chatService";

function AskAIPage()

{
    const [question,
        setQuestion] =
        useState("");

    const [answer,
        setAnswer] =
        useState("");

         const [message, setMessage] =
        useState("");

    const [sources,
        setSources] =
        useState<string[]>([]);

    const [loading,
        setLoading] =
        useState(false);

    async function handleAsk()
{
    const token =
        localStorage.getItem(
            "token");

    if (!token)
    {
        setMessage(
            "Please login before using Ask AI.");

        return;
    }

    if (!question.trim())
    {
        setMessage(
            "Please enter a question.");

        return;
    }

    try
    {
        setLoading(true);

        setMessage("");

        const result =
            await askAI(
                question);

        setAnswer(
            result.answer);

        setSources(
            result.sources);
    }
    catch (error)
    {
        console.error(error);

        setMessage(
            "Failed to get answer.");
    }
    finally
    {
        setLoading(false);
    }
}
    return (
        <div className="container">

            <div className="card shadow">

                <div className="card-body">

                    <h2>
                         Ask AI
                    </h2>
                 {
    message &&
    (
        <div
            className=
            "alert alert-warning"
        >
            {message}
        </div>
    )
}
                    <p className="text-muted">
                        Ask questions about
                        uploaded documents.
                    </p>

                    <textarea
                        className=
                        "form-control mb-3"
                        rows={4}
                        placeholder=
                        "Type your question..."
                        value={question}
                        onChange={e =>
                            setQuestion(
                                e.target.value)}
                    />

                    <button
                        className=
                        "btn btn-primary"
                        onClick={
                            handleAsk
                        }
                        disabled={
                            loading
                        }
                    >
                        {
                            loading
                                ? "Thinking..."
                                : "Send"
                        }
                    </button>

                </div>

            </div>

            {
                answer &&
                (
                    <div
                        className=
                        "card shadow mt-4"
                    >

                        <div
                            className=
                            "card-body"
                        >

                            <h4>
                                AI Answer
                            </h4>

                            <hr />

                            <p>
                                {answer}
                            </p>

                            <h5>
                                Sources
                            </h5>

                            <ul
                                className=
                                "list-group"
                            >
                                {
                                    sources.map(
                                        source =>
                                            (
                                                <li
                                                    key={
                                                        source
                                                    }
                                                    className=
                                                    "list-group-item"
                                                >
                                                    📄 {source}
                                                </li>
                                            ))
                                }
                            </ul>

                        </div>

                    </div>
                )
            }

        </div>
    );
}

export default AskAIPage;