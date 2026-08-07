from app.models.route import Route

from app.services.context_service import ContextService
from app.services.decision_service import DecisionService


class AgentService:

    def __init__(
        self,
        openai_service,
        context_service: ContextService,
        decision_service: DecisionService
    ):

        self._openai_service = openai_service

        self._context_service = context_service

        self._decision_service = decision_service


    async def chat(
        self,
        question: str
    ):

        route = await self._decision_service.decide(
            question
        )


        if route == Route.DIRECT:

            answer = await self._openai_service.ask_direct(
                question
            )

            return {
                "answer": answer,
                "sources": []
            }


        context_response = await self._context_service.get_context(
            question
        )

        chunks = context_response["chunks"]


        context = "\n\n".join(
            chunk["content"]
            for chunk in chunks
        )


        answer = await self._openai_service.ask(
            question,
            context
        )


        sources = list(
            dict.fromkeys(
                chunk["fileName"]
                for chunk in chunks
            )
        )


        return {
            "answer": answer,
            "sources": sources
        }