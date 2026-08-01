from fastapi import FastAPI

app = FastAPI(
    title="KnowledgeHub AI Agent",
    version="1.0.0"
)


@app.get("/")
def health_check():
    return {
        "status": "running",
        "service": "KnowledgeHub AI Agent"
    }