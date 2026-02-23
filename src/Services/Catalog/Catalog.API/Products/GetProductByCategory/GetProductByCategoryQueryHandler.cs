using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    internal class GetProductByCategoryQueryHandler(
        IDocumentSession session,
        ILogger<GetProductByCategoryQueryHandler> logger
    ) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryQuery.Handle called with {@Query}", query);
            var products = await session.Query<Product>()
                .Where(p => p.Categories.Contains(query.Category))
                .ToListAsync(cancellationToken);

            return new GetProductByCategoryResult(products);
        }
    }
}
