﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Hw6
{
    public class CustomExpressionTreeVisitor : ExpressionVisitor
    {
       public Expression<Func<double, double>> GetAnswer(Expression<Func<double, double>> function)
        {
            return Expression.Lambda<Func<double, double>>(
                Visit(function.Body) ?? throw new InvalidOperationException(),
                function.Parameters);
        }

        protected override Expression VisitBinary(BinaryExpression binaryExpr)
            => binaryExpr.NodeType switch
            {
                ExpressionType.Add => Expression.Add(Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"), 
                    Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент")),
                ExpressionType.Subtract => Expression.Subtract(Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"), 
                    Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент")),
                ExpressionType.Multiply => Expression.Multiply(Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"),
                    Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент")),
                ExpressionType.Divide => Expression.Divide(Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"),
                    Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент"))
            };

        protected override Expression VisitConstant(ConstantExpression _) => _;

        protected override Expression VisitParameter(ParameterExpression b) => b;
    }
}