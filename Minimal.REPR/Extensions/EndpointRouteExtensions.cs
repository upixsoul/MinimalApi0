using System.Reflection;

namespace Minimal.REPR.Extensions
{
    public static class EndpointRouteExtensions
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
        {
            var endpointTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in endpointTypes)
            {
                if (Activator.CreateInstance(type) is IEndpoint endpointInstance)
                {
                    endpointInstance.MapEndpoint(app);
                }
            }

            return app;
        }
    }
}
