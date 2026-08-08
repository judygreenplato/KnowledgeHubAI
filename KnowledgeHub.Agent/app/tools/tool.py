class Tool:
    """
    Base class for all tools.

    Every tool should provide:
    - name
    - execute()
    """

    @property
    def name(self) -> str:
        """
        Tool name.

        Child classes should override this.
        """
        raise NotImplementedError(
            "Tool name must be implemented"
        )


    async def execute(
        self,
        question: str
    ):
        """
        Execute the tool.

        Child classes should override this.
        """
        raise NotImplementedError(
            "Execute method must be implemented"
        )