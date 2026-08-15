def test_question_is_not_empty():
    question = "What is KnowledgeHub?"

    assert question != ""


def test_question_is_string():
    question = "Hello"

    assert isinstance(question, str)