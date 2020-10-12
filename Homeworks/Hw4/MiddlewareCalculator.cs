using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hw4
{
    public class MiddlewareCalculator
    {
        private readonly RequestDelegate _next;
        // СДлеать Pattern!! 

        public MiddlewareCalculator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var expression = context.Request.Query["expression"];
            var result = Calculator.Calc(expression);
            if (result == "Error")
                context.Response.StatusCode = 400;
            await _next.Invoke(context);
        }
        //use calculator!!!
    }
}