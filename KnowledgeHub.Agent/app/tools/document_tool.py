from app.tools.tool import Tool
from app.services.context_service import ContextService


class DocumentSearchTool(Tool):

    def __init__(
        self,
        context_service: ContextService
    ):
        self._context_service = context_service


    @property
    def name(self):

        return "search_documents"


    async def execute(
        self,
        question: str
    ):

        return await self._context_service.get_context(
            question
        )