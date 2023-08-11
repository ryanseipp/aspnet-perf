namespace AspnetPerf.MinimalApi.Endpoints.Todos;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder builder)
    {
        TodoEndpoints.MapRoutes(builder);
        return builder;
    }
}
