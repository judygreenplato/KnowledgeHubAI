from app.services.context_service import ContextService


class AgentService:

    def __init__(
        self,
        openai_service,
        context_service: ContextService
    ):
        self._openai_service = openai_service
        self._context_service = context_service


    async def chat(self, question: str):

       
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