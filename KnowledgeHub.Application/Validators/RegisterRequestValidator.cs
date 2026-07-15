using FluentValidation;
using KnowledgeHub.Application.DTOs;

namespace KnowledgeHub.Application.Validators;

public class RegisterRequestValidator
    : AbstractValidator<RegisterUserRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}