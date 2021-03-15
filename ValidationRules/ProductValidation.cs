using FluentValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidation.ValidationRules
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(i => i.Name).NotEmpty().WithMessage("product name can not be empty");
            RuleFor(i => i.Stock).GreaterThanOrEqualTo(0).WithMessage("stock should be min 0");
            RuleFor(i => i.Price).NotEmpty();
            RuleFor(i => i.Name).Length(2, 30).WithMessage("name should be between 2 and 30");
            RuleFor(i => i.CategoryId).NotEmpty().WithMessage("you must select a category name for your product");
        }
    }
}
