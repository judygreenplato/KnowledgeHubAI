from enum import Enum


class Route(Enum):
    """
    Represents the execution route chosen by the AI agent.
    """

    DIRECT = "DIRECT"
    RAG = "RAG"