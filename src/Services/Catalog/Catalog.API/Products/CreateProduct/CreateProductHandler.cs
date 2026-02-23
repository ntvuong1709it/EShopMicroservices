using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string name, List<string> categories, string description, string imageFile, decimal price)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid id);

    public class CreateProductCommandHandler(
        IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new()
            {
                Name = request.name,
                Categories = request.categories,
                Description = request.description,
                ImageFile = request.imageFile,
                Price = request.price
            };

            session.Store(product);
            await session.SaveChangesAsync();

            return new CreateProductResult(product.Id);
        }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Product name is required")
                .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters");

            RuleFor(x => x.categories)
                .NotEmpty().WithMessage("At least one category is required")
                .ForEach(x => x.NotEmpty().WithMessage("Category name cannot be empty"));

            RuleFor(x => x.description)
                .NotEmpty().WithMessage("Product description is required");
            //.Length(10, 500).WithMessage("Product description must be between 10 and 500 characters");

            RuleFor(x => x.imageFile)
                .NotEmpty().WithMessage("Image file is required");
            //.Matches(@"^.*\.(jpg|jpeg|png|gif|webp)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            //.WithMessage("Image file must have a valid image extension (jpg, jpeg, png, gif, webp)");

            RuleFor(x => x.price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
            //.PrecisionScale(10, 2, true).WithMessage("Price must have at most 10 digits with 2 decimal places");
        }
    }
}
