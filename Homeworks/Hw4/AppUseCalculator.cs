﻿using Microsoft.AspNetCore.Builder;

namespace Hw4
{
    public static class AppUseCalculator
    {
        public static void UseCalculator(this IApplicationBuilder app)
        {
            //Add a middleware type to the application's request pipeline.
            app.UseMiddleware<MiddlewareCalculator>();
        }
        
        
    }
}