using Microsoft.AspNetCore.Builder;

namespace Hw7
{
    public static class AppUseCalculator
    {
        public static void UseCalculator(this IApplicationBuilder app)
        {
            app.UseMiddleware<MiddlewareCalculator>();
        }
        
        
    }
}