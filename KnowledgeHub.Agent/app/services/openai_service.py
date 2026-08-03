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

    def ask(self, question: str) -> str:
        """
        Sends a question to OpenAI
        and returns the generated answer.
        """

        response = self._client.chat.completions.create(
            model="gpt-4.1-mini",
            messages=[
                {
                    "role": "user",
                    "content": question
                }
            ]
        )

        return response.choices[0].message.content

    
    