from app.tools.tool import Tool
from app.services.openai_service import OpenAIService


class DirectAnswerTool(Tool):

    def __init__(
        self,
        openai_service: OpenAIService
    ):
        self._openai_service = openai_service


    @property
    def name(self):

        return "direct_answer"


    async def execute(
        self,
        question: str
    ):

        return await self._openai_service.ask_direct(
            question
        )