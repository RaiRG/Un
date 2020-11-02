using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Hw8
{
    public class TaskCreatorWithRequest : ITaskCreator
    {
        private string path = "https://localhost:5001/?expression=";
        public async Task<double> CreateTask(Expression expr,  Dictionary<Expression, Task<double>> BeforeTasks)
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
                var url = path + left.Result + operat + right.Result;
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