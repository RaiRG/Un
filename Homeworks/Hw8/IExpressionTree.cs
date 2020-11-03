using System;
using System.Linq.Expressions;

namespace Hw8
{
    public interface IExpressionTree
    {
        public Expression<Func<double>> CreateExpressionTree(string[] expressionArray);
    }
}