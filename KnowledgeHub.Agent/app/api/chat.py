from fastapi import APIRouter

from app.models.chat_models import ChatRequest, ChatResponse
from app.services.agent_service import AgentService
from app.services.openai_service import OpenAIService

router = APIRouter()

# Create services
openai_service = OpenAIService()
agent_service = AgentService(openai_service)


@router.post("/chat", response_model=ChatResponse)
def chat(request: ChatRequest):

    answer = agent_service.chat(request.question)

    return ChatResponse(answer=answer,sources=[])