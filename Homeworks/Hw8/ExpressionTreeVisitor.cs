using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Hw8
{
    public class ExpressionTreeVisitor : ExpressionVisitor, IGetCalculatedAnswer
    {
        private Expression head;
        private Dictionary<Expression, Task<double>> BeforeTasks = new Dictionary<Expression, Task<double>>();

        private ITaskCreator taskCreator;
        public ExpressionTreeVisitor(ITaskCreator taskCreator)
        {
            this.taskCreator = taskCreator;
        }
        public async Task<double> GetAnswer(Expression<Func<double>> function)
        {
            // Прохожу по всему дереву и заполняю словарь с Task. 
            Visit(function.Body);

            //Запускаю Task
            var result = await BeforeTasks[head];
            return result;
        }

        protected override Expression VisitBinary(BinaryExpression binaryExpr)
        {
            head ??= binaryExpr;
            // Сначала посещаем левую и правую ветку, т.к. только так сможем создать Task.
            Visit(binaryExpr.Left);
            Visit(binaryExpr.Right);
            BeforeTasks.Add(binaryExpr, taskCreator.CreateTask(binaryExpr, BeforeTasks));
            return binaryExpr;
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            BeforeTasks.Add(constantExpression, taskCreator.CreateTask(constantExpression, BeforeTasks));
            return constantExpression;
        }
        
    }
}