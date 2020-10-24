using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hw4
{
    public class MiddlewareCalculator
    {
        private readonly RequestDelegate _next;
        public MiddlewareCalculator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var expression = context.Request.Query["expression"];
            var result = Calculator.Calc(expression);
            if (result == "Error")
            {
                await context.Response.WriteAsync("Invalid data format!");
                //context.Response.StatusCode = 400;
            }
            else
            {
                context.Response.StatusCode = 200;
                context.Response.Headers.Add("result", result);
                await context.Response.WriteAsync(result);
            }
            await _next(context);
        }
    }
}