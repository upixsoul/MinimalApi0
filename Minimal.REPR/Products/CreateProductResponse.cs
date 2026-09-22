namespace Minimal.REPR.Products
{
    public sealed record CreateProductResponse(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    int Stock,
    DateTime CreatedAtUtc);
}
