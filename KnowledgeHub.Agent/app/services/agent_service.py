from app.services.openai_service import OpenAIService


class AgentService:
    """
    Coordinates the AI Agent workflow.

    Currently it forwards the user's question
    to OpenAI.

    Future versions will:
    - Use RAG
    - Use Memory
    - Call external tools
    - Plan multi-step tasks
    """

    def __init__(self, openai_service: OpenAIService):
        self._openai_service = openai_service

    def chat(self, question: str) -> str:
        """
        Processes the user's question
        and returns the AI response.
        """

        return self._openai_service.ask(question)