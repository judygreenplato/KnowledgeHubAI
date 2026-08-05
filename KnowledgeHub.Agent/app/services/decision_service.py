from app.models.route import Route


class DecisionService:
    """
    Decides which execution path the AI agent should use.
    """

    async def decide(self, question: str) -> Route:
        

        return Route.RAG