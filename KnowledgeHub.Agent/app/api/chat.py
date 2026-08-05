from fastapi import APIRouter

from app.models.chat_models import ChatRequest, ChatResponse
from app.services.agent_service import AgentService
from app.services.openai_service import OpenAIService
from app.services.context_service import ContextService

router = APIRouter()

# Create services
openai_service = OpenAIService()
context_service = ContextService()

agent_service = AgentService(
    openai_service,
    context_service
)


@router.post("/chat", response_model=ChatResponse)
async def chat(request: ChatRequest):

    result = await agent_service.chat(
        request.question
    )

    return ChatResponse(
        answer=result["answer"],
        sources=result["sources"]
    )