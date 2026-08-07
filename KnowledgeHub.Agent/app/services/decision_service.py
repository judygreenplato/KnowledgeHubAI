from app.models.route import Route
from app.services.openai_service import OpenAIService


class DecisionService:

    def __init__(
        self,
        openai_service: OpenAIService
    ):
        self._openai_service = openai_service


    async def decide(
        self,
        question: str
    ) -> Route:

        question = question.lower().strip()

        direct_questions = {
            "hi",
            "hello",
            "hey",
            "thanks",
            "thank you",
            "good morning",
            "good evening"
        }

        if question in direct_questions:
            return Route.DIRECT

        decision = await self._openai_service.decide_route(
            question
        )

        if decision == "RAG":
            return Route.RAG

        return Route.DIRECT