using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hw8
{
    public class ExpressionTree : IExpressionTree
    {
        // Создает дерево выражений по введенному математическому выражению.
        public Expression<Func<double>> CreateExpressionTree(string[] expressionArray)
        {
            var posfixExpression = infixToPostfix(expressionArray);
            var stack = new Stack<Expression>();
            for (var i = 0; i < posfixExpression.Count; i++)
            {
                var symbol = posfixExpression[i];
                if (symbol == "")
                    continue;
                // Если это число, переводим в константу и пушим
                if (symbol != "+" && symbol != "-" && symbol != "*" && symbol != "/")
                {
                    stack.Push(Expression.Constant(double.Parse(symbol)));
                }
                // Иначе строим ветвть() дерева
                else
                {
                    Expression operand1 = stack.Pop();
                    Expression operand2 = stack.Pop();
                    switch (symbol)
                    {
                        case "+":
                            stack.Push(Expression.Add(operand2, operand1));
                            break;
                        case "-":
                            stack.Push(Expression.Subtract(operand2, operand1));
                            break;
                        case "*":
                            stack.Push(Expression.Multiply(operand2, operand1));
                            break;
                        case "/":
                            stack.Push(Expression.Divide(operand2, operand1));
                            break;
                    }
                }
            }

            Expression result = stack.Pop();
            
            Expression<Func<double>> function
                = Expression.Lambda<Func<double>> (result);
            return function;
        }
        
          
        private static List<string> infixToPostfix(string[] inputExpression)
        {
            var result = new List<string>();
            Stack<string> stack = new Stack<string>();
            for (int i = 0; i < inputExpression.Length; ++i)
            {
                var currentSymbol = inputExpression[i];
                double number;
                // If the scanned character is an operand, add it to output.
                if (double.TryParse(currentSymbol, out number))
                    result.Add(currentSymbol.ToString());
                else
                {
                    // If the scanned character is an '(', push it to the stack.  
                    if (currentSymbol == "(")
                    {
                        stack.Push(currentSymbol.ToString());
                    }

                    //  If the scanned character is an ')', pop and output from the stack   
                    // until an '(' is encountered.  
                    else if (currentSymbol == ")")
                    {
                        while (stack.Count > 0 && stack.Peek() != "(")
                        {
                            result.Add(stack.Pop());
                        }

                        if (stack.Count > 0 && stack.Peek() != "(")
                        {
                            throw new Exception("Invalid Expression"); // invalid expression 
                        }
                        else
                        {
                            stack.Pop();
                        }
                    }
                    else // an operator is encountered 
                    {
                        while (stack.Count > 0 && Prec(currentSymbol.ToString()) <= Prec(stack.Peek()))
                        {
                            result.Add(stack.Pop());
                        }

                        stack.Push(currentSymbol.ToString());
                    }
                }
            }

            // pop all the operators from the stack  
            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }

            return result;
        }
        //возвращает приоритет заданного оператора
        // Более высокое возвращаемое значение означает более высокий приоритет
        private static int Prec(string ch)
        {
            switch (ch)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
            }
            return -1;
        }

    }
}