using bejebeje.admin.Application.Lyrics.Commands.CreateLyric;
using FluentValidation;

namespace bejebeje.admin.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateLyricCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
