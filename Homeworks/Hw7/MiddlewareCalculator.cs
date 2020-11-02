using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hw7
{
    public class MiddlewareCalculator
    {
        private readonly RequestDelegate _next;
        private readonly ICalculator _calc;
        
        public MiddlewareCalculator(RequestDelegate next, ICalculator calculator)
        {
            _next = next;
            _calc = calculator;
        }

        // Кальялкутор для middlaware - зависимость.
        public async Task InvokeAsync(HttpContext context)
        {
            var expression = context.Request.Query["expression"];
            var result = _calc.CalculateExpression(expression);
            
            if (result == "Error")
            {
                await context.Response.WriteAsync("Invalid data format!");
                context.Response.StatusCode = 400;
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