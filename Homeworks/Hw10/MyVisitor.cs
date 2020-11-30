using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Hw10
{
    public class ExpressionTreeVisitor
    {
        private string path = "https://localhost:5001/?expression=";
        private Expression head;
        private Dictionary<Expression, Task<double>> BeforeTasks = new Dictionary<Expression, Task<double>>();

        public void Visit(dynamic node)
        {
            DynamicVisit(node);
        }

        protected Expression DynamicVisit(BinaryExpression binaryExpr)
        {
            head ??= binaryExpr;
            // Сначала посещаем левую и правую ветку, т.к. только так сможем создать Task.
            Visit(binaryExpr.Left);
            Visit(binaryExpr.Right);
            BeforeTasks.Add(binaryExpr, CreateTask(binaryExpr));
            return binaryExpr;
        }

        protected Expression DynamicVisit(ConstantExpression constantExpression)
        {
            BeforeTasks.Add(constantExpression, CreateTask(constantExpression));
            return constantExpression;
        }
        
        public async Task<double> GetAnswer(Expression<Func<double>> function)
        {
            // Прохожу по всему дереву и заполняю словарь с Task. 
            Visit(function.Body);

            //Запускаю Task
            var result = await BeforeTasks[head];
            return result;
        }

      

        private async Task<double> CreateTask(Expression expr)
        {
            return await Task.Run(() =>
            {
                if (expr is ConstantExpression expression)
                {
                    return (double) expression.Value;
                }

                var currentBinaryExpression = (BinaryExpression) expr;
                // Получаем Task с двух веток. 
                var left = BeforeTasks[currentBinaryExpression.Left];
                var right = BeforeTasks[currentBinaryExpression.Right];

                // Запускаются обе ветки.
                Task.WhenAll(left, right);
                var operat = GetOperatorForURL(expr);
                var leftOperand = left.Result;
                var rightOperand = right.Result;
                var url = path + leftOperand + operat + rightOperand;
                var req = HttpWebRequest.Create(url.ToString());
                var rsp = (HttpWebResponse) req.GetResponse();
                if (Convert.ToInt32(rsp.StatusCode) != 200)
                {
                    throw new Exception("От сервера ошибка");
                }

                var result = rsp.Headers.GetValues("result")?[0];
                Console.WriteLine(left.Result + GetOperatorForConsolePrint(expr) + right.Result + "=" +
                                  result.ToString());
                if (result != null)
                {
                    return double.Parse(result);
                }

                Console.WriteLine("Ошибка сервера");
                throw new Exception("От сервера null");
            });
        }

        private string GetOperatorForURL(Expression currentBinaryExpression)
        {
            return currentBinaryExpression.NodeType switch
            {
                ExpressionType.Add => "%2B",
                ExpressionType.Subtract => "-",
                ExpressionType.Multiply => "*",
                ExpressionType.Divide => "%2F",
                _ => "%2B"
            };
        }

        private string GetOperatorForConsolePrint(Expression currentBinaryExpression)
        {
            return currentBinaryExpression.NodeType switch
            {
                ExpressionType.Add => "+",
                ExpressionType.Subtract => "-",
                ExpressionType.Multiply => "*",
                ExpressionType.Divide => "/",
                _ => "+"
            };
        }
    }
}