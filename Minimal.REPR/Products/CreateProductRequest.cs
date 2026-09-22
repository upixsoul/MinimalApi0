namespace Minimal.REPR.Products
{
    public sealed record CreateProductRequest(
    string Name,
    string Sku,
    decimal Price,
    int Stock);
}
