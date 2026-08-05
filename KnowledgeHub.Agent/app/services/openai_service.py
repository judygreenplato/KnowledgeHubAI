from openai import OpenAI

from app.core.settings import settings


class OpenAIService:
    """
    Service responsible for communicating with OpenAI.
    """

    def __init__(self):

        self._client = OpenAI(
            api_key=settings.openai_api_key
        )

    async def ask(
        self,
        question: str,
        context: str
    ) -> str:
        """
        Sends the question together with the retrieved
        document context to OpenAI and returns the answer.
        """

        response = self._client.chat.completions.create(
            model="gpt-4.1-mini",
            messages=[
                {
                    "role": "system",
                    "content":
                        (
                            "You are an AI assistant for KnowledgeHub. "
                            "Answer the user's question ONLY using the "
                            "provided context. "
                            "If the answer cannot be found in the context, "
                            "say that the information is not available "
                            "in the uploaded documents."
                        )
                },
                {
                    "role": "user",
                    "content":
                        f"""Context:

{context}

Question:

{question}
"""
                }
            ]
        )

        return response.choices[0].message.content