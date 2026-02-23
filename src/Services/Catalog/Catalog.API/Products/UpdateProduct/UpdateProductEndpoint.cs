namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(string name, List<string> categories, string description, string imageFile, decimal price);
    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductRequest request, ISender sender) =>
            {
                var command = new UpdateProductCommand(id, request.name, request.categories, request.description, request.imageFile, request.price);
                var result = await sender.Send(command);
                var response = new UpdateProductResponse(result.IsSuccess);
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update product")
            .WithDescription("Update product description");
        }
    }
}