using FluentValidation;

namespace TaskManager.Application.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Task Id is required");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value");
    }
}