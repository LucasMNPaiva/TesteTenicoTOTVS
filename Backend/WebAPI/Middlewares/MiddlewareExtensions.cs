namespace WebAPI.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection services)
        {
            services.AddProblemDetails();
            services.AddTransient<ErrorHandlingMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder app)
        {
            // Posicione cedo no pipeline
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
