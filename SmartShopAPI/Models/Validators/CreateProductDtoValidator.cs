using FluentValidation;
using SmartShopAPI.Data;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Models.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Price).PrecisionScale(8, 2, false)
                .WithMessage("Price must not be more than 8 digits in total and 2 decimal");
            RuleFor(x => x.Name).MaximumLength(50);
        }
    }
}
