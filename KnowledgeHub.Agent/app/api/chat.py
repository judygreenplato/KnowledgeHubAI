from fastapi import APIRouter

from app.models.chat_models import ChatRequest, ChatResponse
from app.services.agent_service import AgentService
from app.services.openai_service import OpenAIService
from app.services.context_service import ContextService
from app.services.decision_service import DecisionService

router = APIRouter()

# Create services
openai_service = OpenAIService()
context_service = ContextService()
decision_service = DecisionService()

agent_service = AgentService(
    openai_service,
    context_service,
    decision_service
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