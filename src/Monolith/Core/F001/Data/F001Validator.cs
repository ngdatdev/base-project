using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Entities;
using Common.Features;
using FluentValidation;

namespace F001.Data
{
    internal class F001Validator : Validator<F001Request, F001Response>
    {
        public F001Validator()
        {
            RuleFor(expression: request => request.Name)
                .NotEmpty()
                .MaximumLength(maximumLength: 100)
                .MinimumLength(minimumLength: 10);
        }
    }
}
