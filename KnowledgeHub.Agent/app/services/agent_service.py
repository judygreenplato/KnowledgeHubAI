class AgentService:
    """
    AI Agent responsible for deciding
    which tool should handle the user's question.
    """

    def __init__(
        self,
        openai_service,
        tools
    ):
        self._openai_service = openai_service
        self._tools = tools

    async def chat(
        self,
        question: str
    ):

        # Step 1:
        # Ask OpenAI which tool should be used.
        tool_name = await self._openai_service.choose_tool(
            question
        )

        print(f"Selected Tool: {tool_name}")

        # Step 2:
        # Find the selected tool.
        selected_tool = next(
            (
                tool
                for tool in self._tools
                if tool.name == tool_name
            ),
            None
        )

        if selected_tool is None:
            raise Exception(
                f"Unknown tool: {tool_name}"
            )

        # Step 3:
        # Execute the selected tool.
        result = await selected_tool.execute(
            question
        )

        # -----------------------------------
        # DIRECT ANSWER TOOL
        # -----------------------------------

        if tool_name == "direct_answer":

            return {
                "answer": result,
                "sources": []
            }

        # -----------------------------------
        # DOCUMENT SEARCH TOOL
        # -----------------------------------

        chunks = result["chunks"]

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