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
        Generates an answer using the provided context.
        """

        response = self._client.chat.completions.create(
            model="gpt-4.1-mini",
            messages=[
                {
                    "role": "system",
                    "content":
                    (
                        "You are a helpful AI assistant.\n\n"
                        "Answer the user's question ONLY using the provided context.\n"
                        "If the answer cannot be found in the context, "
                        "say that the information is not available "
                        "in the uploaded documents."
                    )
                },
                {
                    "role": "user",
                    "content":
                    (
                        f"Context:\n\n{context}\n\n"
                        f"Question:\n{question}"
                    )
                }
            ]
        )

        return response.choices[0].message.content

    async def decide_route(
        self,
        question: str
    ) -> str:
        """
        Uses OpenAI to decide whether the question
        requires document retrieval (RAG) or can be
        answered directly.
        """

        response = self._client.chat.completions.create(
            model="gpt-4.1-mini",
            temperature=0,
            messages=[
                {
                    "role": "system",
                    "content":
                    (
                        "You are an AI routing assistant.\n\n"
                        "Your job is NOT to answer the user's question.\n"
                        "Your only job is to decide whether the question "
                        "requires information from uploaded documents.\n\n"
                        "Return ONLY one word:\n\n"
                        "RAG\n"
                        "DIRECT\n\n"
                        "Choose RAG if uploaded documents are needed.\n"
                        "Choose DIRECT if the question can be answered "
                        "without uploaded documents."
                    )
                },
                {
                    "role": "user",
                    "content": question
                }
            ]
        )

        return response.choices[0].message.content.strip()