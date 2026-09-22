namespace Minimal.REPR.Products
{
    // Response: Individual product DTO
    public sealed record ProductItemResponse(
        Guid Id,
        string Name,
        string Sku,
        decimal Price,
        int Stock,
        DateTime CreatedAtUtc);
}
