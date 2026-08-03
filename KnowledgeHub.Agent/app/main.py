
from fastapi import FastAPI

from app.api.chat import router as chat_router

app = FastAPI(
    title="KnowledgeHub AI Agent",
    version="1.0.0"
)


@app.get("/health")
def health():
    return {"status": "healthy"}


app.include_router(chat_router)