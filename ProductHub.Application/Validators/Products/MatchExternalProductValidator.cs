using FluentValidation;
using ProductHub.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Validators.Products;

public class MatchExternalProductRequestValidator : AbstractValidator<MatchExternalProductRequest>
{
    public MatchExternalProductRequestValidator()
    {
        RuleFor(x => x.ExternalProductId)
            .GreaterThan(0);
    }
}