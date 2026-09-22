using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Minimal.REPR.Models;

namespace Minimal.REPR.Products
{
    public sealed class CreateProductEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/products", HandleAsync)
               .WithName("CreateProduct")
               .WithTags("Products")
               .WithSummary("Creates a new product")
               .Produces<CreateProductResponse>(StatusCodes.Status201Created)
               .ProducesValidationProblem(StatusCodes.Status400BadRequest);
        }

        // Endpoint handler (Single Responsibility)
        public static async Task<Results<Created<CreateProductResponse>, ValidationProblem>> HandleAsync(
            CreateProductRequest request,
            IValidator<CreateProductRequest> validator,
            AppDbContext dbContext,
            // Inject your services, DbContext or repository here
            CancellationToken cancellationToken)
        {
            // 1. Validate the request
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            // 2. Execute business logic / persistence
            // (Example of persisting the entity)
            var productId = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;

            var response = new CreateProductResponse(
                productId,
                request.Name,
                request.Sku,
                request.Price,
                request.Stock,
                createdAt
            );

            // Persist using EF Core
            var product = new Product
            {
                Id = productId,
                Name = request.Name,
                Sku = request.Sku,
                Price = request.Price,
                Stock = request.Stock,
                CreatedAtUtc = createdAt
            };

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync(cancellationToken);
            // 3. Return the appropriate response with TypedResults
            return TypedResults.Created($"/api/products/{productId}", response);
        }
    }
}
