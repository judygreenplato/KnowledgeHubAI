import { useEffect, useState }
    from "react";

import type { Dashboard }
    from "../models/dashboard";

import { getDashboard }
    from "../services/dashboardService";

function DashboardPage()
{
    const [dashboard,
        setDashboard] =
        useState<Dashboard | null>(
            null);

    const [loading,
        setLoading] =
        useState(true);

    useEffect(() =>
    {
        loadDashboard();
    }, []);

    async function loadDashboard()
    {
        try
        {
            const data =
                await getDashboard();

            setDashboard(data);
        }
        catch (error)
        {
            console.error(error);
        }
        finally
        {
            setLoading(false);
        }
    }

    if (loading)
    {
        return (
            <h3>
                Loading Dashboard...
            </h3>
        );
    }

    return (
        <div className="container">

            <h1 className="mb-4">
                KnowledgeHub Dashboard
            </h1>

            <div className="row">

                <div className="col-md-4">

                    <div
                        className=
                        "card text-center shadow"
                    >
                        <div
                            className=
                            "card-body"
                        >
                            <h5>
                                Articles
                            </h5>

                            <h2>
                                {
                                    dashboard
                                        ?.articles
                                }
                            </h2>
                        </div>
                    </div>

                </div>
                   <div className="col-md-4">

                    <div
                        className=
                        "card text-center shadow"
                    >
                        <div
                            className=
                            "card-body"
                        >
                            <h5>
                                Categories
                            </h5>

                            <h2>
                                {
                                    dashboard
                                        ?.categories
                                }
                            </h2>
                        </div>
                    </div>

                </div>
                
                <div className="col-md-4">

                    <div
                        className=
                        "card text-center shadow"
                    >
                        <div
                            className=
                            "card-body"
                        >
                            <h5>
                                Documents
                            </h5>

                            <h2>
                                {
                                    dashboard
                                        ?.documents
                                }
                            </h2>
                        </div>
                    </div>

                </div>
                 
                <div className="col-md-4">

                    <div
                        className=
                        "card text-center shadow"
                    >
                        <div
                            className=
                            "card-body"
                        >
                            <h5>
                                Embeddings
                            </h5>

                            <h2>
                                {
                                    dashboard
                                        ?.embeddings
                                }
                            </h2>
                        </div>
                    </div>

                </div>


                <div className="col-md-4">

                    <div
                        className=
                        "card text-center shadow"
                    >
                        <div
                            className=
                            "card-body"
                        >
                            <h5>
                                Users
                            </h5>

                            <h2>
                                {
                                    dashboard
                                        ?.users
                                }
                            </h2>
                        </div>
                    </div>

                </div>

            </div>

        </div>
    );
}

export default DashboardPage;