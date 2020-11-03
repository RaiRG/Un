using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Hw8;
using Microsoft.Extensions.DependencyInjection;

namespace Hw8.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("( 2 + 3 ) / 12 * 7 + 8 * 9")]
        [InlineData("1 * 2 * 3 * 4 * 5 * 6")]
        [InlineData("1 + ( 2 + ( 3 + ( 4 + ( 5 * 6 ) * 8 ) * 10 ) / 13 + 4 )")]
        [InlineData("5 * 2")]
        [InlineData("( 2 + 3 ) * ( 4 - 3 ) / ( 12 * 6 )")]
        [InlineData("( ( ( ( ( ( ( 8 / 4 ) ) ) ) ) ) )")]
        [InlineData("1000 - ( 1 + ( 2 + ( 3 + ( 4 + ( 6 + ( 7 + ( 8 + 9 ) + 10 ) + 11 ) + 12 ) + 13 ) + 14 ) - 15 )")]
        [InlineData("1 * 4 + 2 * 3")]
        [InlineData("1000 / 2 / 3 / 4 / 5 / 6 / 7 / 8 / 9")]
        [InlineData("5 + ( 9 / 12 + 3 )")]
        [InlineData("14 - 16 / 2")]
        [InlineData("( 1 + ( 2 * ( 3 + 5 ) / 19 ) )")]
        [InlineData("1 - ( 2 * ( 3 - 5 ) / 19 ) ")]
        [InlineData("1 - ( 3 - 4 ) * ( 5 / 19 ) - 16 + 900 - 4")]
        
        
        public void Test1(string inputExpression)
        {
            
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<ICalculator, Calculator>();
            services.AddScoped<IExpressionTree, ExpressionTree>();
            services.AddScoped<ITaskCreator, TaskCreatorWithoutRequest>();
            services.AddScoped<IGetCalculatedAnswer, ExpressionTreeVisitor>();

            var provider = services.BuildServiceProvider();
            var calculator = provider.GetService<ICalculator>();
            var expressionTreeBuilder = provider.GetService<IExpressionTree>();
            
            var expression = expressionTreeBuilder.CreateExpressionTree(inputExpression.Split(" "));
           
            var expected = expression.Compile()();
            var actual = calculator.CalculateExpression(inputExpression);
           
            Assert.Equal(actual, expected.ToString());
        }
    }

}