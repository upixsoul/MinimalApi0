using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Minimal.REPR.Products
{
    public sealed class GetProductsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/products", HandleAsync)
               .WithName("GetProducts")
               .WithTags("Products")
               .WithSummary("Gets a paginated list of products")
               .Produces<GetProductsResponse>(StatusCodes.Status200OK);
        }

        public static async Task<Ok<GetProductsResponse>> HandleAsync(
            [AsParameters] GetProductsRequest request, // Mapea QueryString automáticamente
            AppDbContext dbContext,
            CancellationToken cancellationToken)
        {
            // 1. Sanitize pagination
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            // 2. Count total records (useful for frontend to calculate number of pages)
            var totalCount = await dbContext.Products.CountAsync(cancellationToken);

            // 3. Query the in-memory DB with EF Core
            var items = await dbContext.Products
                .AsNoTracking() // IMPORTANT: Improves performance for read-only queries
                .OrderByDescending(p => p.CreatedAtUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductItemResponse( // Proyección directa a DTO
                    p.Id,
                    p.Name,
                    p.Sku,
                    p.Price,
                    p.Stock,
                    p.CreatedAtUtc))
                .ToListAsync(cancellationToken);

            // 4. Build and return the response
            var response = new GetProductsResponse(items, totalCount, page, pageSize);

            return TypedResults.Ok(response);
        }
    }
}
