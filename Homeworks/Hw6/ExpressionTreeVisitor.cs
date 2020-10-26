﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Hw6
{
    public class ExpressionTreeVisitor : ExpressionVisitor
    {
        public Expression<Func<double>> GetAnswer(Expression<Func<double>> function)
        {
            var z = Expression.Lambda<Func<double>>(
                Visit(function.Body) ?? throw new InvalidOperationException(),
                function.Parameters);
            return Expression.Lambda<Func<double>>(
                Visit(function.Body) ?? throw new InvalidOperationException(),
                function.Parameters);
        }

       protected override Expression VisitBinary(BinaryExpression binaryExpr)
       {
           Expression res;
           switch (binaryExpr.NodeType)
           {
               case ExpressionType.Add:
                   res = Expression.Add(
                       Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"),
                       Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент"));
                   break;
               case ExpressionType.Subtract:
                   res = Expression.Subtract(
                       Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"),
                       Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент"));
                   break;
               case ExpressionType.Multiply:
                   res = Expression.Multiply(
                       Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"),
                       Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент"));
                   break;
               case ExpressionType.Divide:
                   res = Expression.Divide(
                       Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"),
                       Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент"));
                   break;
               default:
                   res = Expression.Add(
                       Visit(binaryExpr.Left) ?? throw new Exception("Неверный аргумент"),
                       Visit(binaryExpr.Right) ?? throw new Exception("Неверный аргумент"));
                   break;
           }
           return res;
       }
        protected override Expression VisitConstant(ConstantExpression _) => _;

        protected override Expression VisitParameter(ParameterExpression b) => b;
    }
}