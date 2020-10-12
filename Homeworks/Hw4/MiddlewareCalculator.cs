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
            _next.Invoke(context);
            
        }
        //use calculator!!!
    }
}