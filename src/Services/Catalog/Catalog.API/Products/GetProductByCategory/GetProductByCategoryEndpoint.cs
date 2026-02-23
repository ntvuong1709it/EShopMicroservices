
using Catalog.API.Models;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public record GetProductsResponse(IEnumerable<Product> Products);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
             app.MapGet("/products/category/{category}", async (ISender sender, string category) =>
             {
                 var query = new GetProductByCategoryQuery(category);
                 var result = await sender.Send(query);
                 var response = new GetProductByCategoryResponse(result.Products);
                 return Results.Ok(response);
             })
             .WithName("GetProductByCategory")
             .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Get products by category")
             .WithDescription("Get products by category description");
        }
    }
}
