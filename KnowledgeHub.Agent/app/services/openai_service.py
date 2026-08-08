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
        Generates an answer using the provided document context.
        """

        response = self._client.chat.completions.create(
            model="gpt-4.1-mini",
            messages=[
                {
                    "role": "system",
                    "content":
                    (
                        "You are a helpful AI assistant.\n\n"
                        "Answer ONLY using the provided document context.\n"
                        "If the answer is not found in the context, "
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

    async def ask_direct(
        self,
        question: str
    ) -> str:
        """
        Answers a general question without using uploaded documents.
        """

        response = self._client.chat.completions.create(
            model="gpt-4.1-mini",
            messages=[
                {
                    "role": "system",
                    "content":
                    (
                        "You are a helpful AI assistant."
                    )
                },
                {
                    "role": "user",
                    "content": question
                }
            ]
        )

        return response.choices[0].message.content

    async def choose_tool(
        self,
        question: str
    ) -> str:
        """
        Decide which tool should handle the user's question.

        Returns ONLY one of:

        search_documents
        direct_answer
        """

        response = self._client.chat.completions.create(
            model="gpt-4.1-mini",
            temperature=0,
            messages=[
                {
                    "role": "system",
                    "content":
                    (
                        "You are an AI tool selection assistant.\n\n"

                        "Your job is NOT to answer the user's question.\n\n"

                        "Your ONLY job is to choose ONE tool.\n\n"

                        "Available tools:\n\n"

                        "search_documents\n"
                        "direct_answer\n\n"

                        "Choose 'search_documents' when the answer "
                        "depends on uploaded documents.\n\n"

                        "Choose 'direct_answer' when the question "
                        "can be answered without uploaded documents.\n\n"

                        "Return ONLY the tool name.\n"

                        "Do not explain your decision."
                    )
                },
                {
                    "role": "user",
                    "content": question
                }
            ]
        )

        return response.choices[0].message.content.strip()