using FluentValidation;

namespace TaskSync.Domain.Shared;

public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
{
    public PaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber).NotNull().GreaterThan(0);
        RuleFor(x => x.PageSize).NotNull().GreaterThan(0).LessThanOrEqualTo(100);
    }
}

