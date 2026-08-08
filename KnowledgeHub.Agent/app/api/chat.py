from fastapi import APIRouter

from app.models.chat_models import ChatRequest, ChatResponse

from app.services.agent_service import AgentService
from app.services.openai_service import OpenAIService
from app.services.context_service import ContextService

from app.tools.document_tool import DocumentSearchTool
from app.tools.direct_tool import DirectAnswerTool


router = APIRouter()


# Services

openai_service = OpenAIService()

context_service = ContextService()


# Tools

document_tool = DocumentSearchTool(
    context_service
)


direct_tool = DirectAnswerTool(
    openai_service
)


# Agent

agent_service = AgentService(
    openai_service,
    [
        document_tool,
        direct_tool
    ]
)



@router.post(
    "/chat",
    response_model=ChatResponse
)
async def chat(request: ChatRequest):

    result = await agent_service.chat(
        request.question
    )


    return ChatResponse(
        answer=result["answer"],
        sources=result["sources"]
    )