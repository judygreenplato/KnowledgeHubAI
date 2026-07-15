using FluentValidation;
using KnowledgeHub.Application.DTOs;

namespace KnowledgeHub.Application.Validators;

public class CreateArticleRequestValidator
    : AbstractValidator<CreateArticleRequest>
{
    public CreateArticleRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required.");
    }
}