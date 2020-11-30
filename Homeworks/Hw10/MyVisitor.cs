using System.Linq.Expressions;

namespace Hw10
{
    public class MyVisitor : ExpressionVisitor
    {
        public Expression Visit(dynamic node)
        {
            return VisitExpression(node);
        }

        private Expression VisitExpression(BinaryExpression expression)
        {
            Visit(expression.Right);
            Visit(expression.Left);
            return expression;
        }
        private Expression VisitExpression(ConstantExpression expression)
        {
            
            //
        }
       
        
    }
}