import { useEffect, useState } from "react";
import api from "../api/axios";

interface DocumentItem {
    id: string;
    fileName: string;
    fileSize: number;
    uploadedAtUtc: string;
}

function DocumentsPage() {

    const [file, setFile] =
        useState<File | null>(null);

    const [documents, setDocuments] =
        useState<DocumentItem[]>([]);

    const [message, setMessage] =
        useState("");

    const [loading, setLoading] =
        useState(false);

    useEffect(() => {

        loadDocuments();

    }, []);

    async function loadDocuments() {

        try {

            const response =
                await api.get<DocumentItem[]>(
                    "/documents");

            setDocuments(
                response.data);
        }
        catch (error) {

            console.error(error);
        }
    }

    async function upload() {

        if (!file) {

            setMessage(
                "Please select a file.");

            return;
        }

        try {

            setLoading(true);

            const formData =
                new FormData();

            formData.append(
                "file",
                file);

            await api.post(
                "/documents/upload",
                formData);

            setMessage(
                "Document uploaded successfully.");

            await loadDocuments();

            setFile(null);
        }
        catch (error) {

            console.error(error);

            setMessage(
                "Document upload failed.");
        }
        finally {

            setLoading(false);
        }
    }

    function formatFileSize(
        bytes: number)
    {
        return (
            bytes / 1024
        ).toFixed(2) + " KB";
    }

    return (

        <div className="container">

            <h1 className="mb-4">
                Documents
            </h1>

            <div className="card shadow mb-4">

                <div className="card-body">

                    <h3>
                        Upload Document
                    </h3>

                    {
                        message &&
                        (
                            <div
                                className=
                                "alert alert-info mt-3"
                            >
                                {message}
                            </div>
                        )
                    }

                    <div className="mb-3">

                        <input
                            type="file"
                            className="form-control"
                            onChange={e =>
                                setFile(
                                    e.target.files?.[0]
                                    ?? null)}
                        />

                    </div>

                    <button
                        className=
                        "btn btn-primary"
                        onClick={upload}
                        disabled={loading}
                    >
                        {
                            loading
                                ? "Uploading..."
                                : "Upload"
                        }
                    </button>

                </div>

            </div>

            <div className="card shadow">

                <div className="card-body">

                    <h3 className="mb-3">
                        Uploaded Documents
                    </h3>

                    {
                        documents.length === 0
                            ? (
                                <p>
                                    No documents found.
                                </p>
                            )
                            : (
                                <table
                                    className=
                                    "table table-striped table-hover"
                                >

                                    <thead>

                                        <tr>

                                            <th>
                                                File Name
                                            </th>

                                            <th>
                                                Size
                                            </th>

                                            <th>
                                                Uploaded Date
                                            </th>

                                        </tr>

                                    </thead>

                                    <tbody>

                                        {
                                            documents.map(
                                                document => (

                                                    <tr
                                                        key={
                                                            document.id
                                                        }
                                                    >

                                                        <td>
                                                            {
                                                                document.fileName
                                                            }
                                                        </td>

                                                        <td>
                                                            {
                                                                formatFileSize(
                                                                    document.fileSize)
                                                            }
                                                        </td>

                                                        <td>
                                                            {
                                                                new Date(
                                                                    document.uploadedAtUtc)
                                                                    .toLocaleString()
                                                            }
                                                        </td>

                                                    </tr>

                                                ))
                                        }

                                    </tbody>

                                </table>
                            )
                    }

                </div>

            </div>

        </div>

    );
}

export default DocumentsPage;