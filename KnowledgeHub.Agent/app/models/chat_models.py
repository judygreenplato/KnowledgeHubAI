from pydantic import BaseModel


class ChatRequest(BaseModel):
    """
    Represents a chat request sent by the client.
    """

    question: str


class ChatResponse(BaseModel):
    """
    Represents the response returned to the client.
    """

    answer: str
    sources: list[str] = []