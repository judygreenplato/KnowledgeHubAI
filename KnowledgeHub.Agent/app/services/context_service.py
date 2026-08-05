import httpx

from app.core.settings import settings


class ContextService:
    """
    Responsible for retrieving document context
    from the ASP.NET Core API.
    """

    async def get_context(self, question: str):

        async with httpx.AsyncClient() as client:

            response = await client.post(
                f"{settings.dotnet_api_url}/api/rag/context",
                json={
                    "question": question
                }
            )

            response.raise_for_status()

            return response.json()