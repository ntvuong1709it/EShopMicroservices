using Catalog.API.Models;

namespace Catalog.API.Products.GetProductById
{
    //public record GetProductByIdRequest(Guid id);
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);
                return Results.Ok(new GetProductByIdResponse(result.Product));
            });
        }
    }
}
