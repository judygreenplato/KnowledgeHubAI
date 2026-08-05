import os

from dotenv import load_dotenv

load_dotenv()


class Settings:
    def __init__(self):
        self.openai_api_key = os.getenv("OPENAI_API_KEY")
        self.dotnet_api_url = os.getenv("DOTNET_API_URL")


settings = Settings()