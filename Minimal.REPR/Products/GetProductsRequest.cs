namespace Minimal.REPR.Products
{
    // Request: QueryString parameters
    public sealed record GetProductsRequest(int Page = 1, int PageSize = 10);
}
