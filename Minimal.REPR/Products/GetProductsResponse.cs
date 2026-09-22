namespace Minimal.REPR.Products
{
    // Response: Paged wrapper
    public sealed record GetProductsResponse(
        IEnumerable<ProductItemResponse> Items,
        int TotalCount,
        int Page,
        int PageSize);
}
