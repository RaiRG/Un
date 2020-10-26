using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Hw6
{
    public class Calculator
    {
        private static readonly char[] operators = new char[]
        {
            '+', '-', '*', '/'
        };

        private static bool isUnputStringCorrect(string[] inputExpressionArray)
        {
            var previous = 1;  // 1 - число, 0 - оператор
            for (var index = 0; index < inputExpressionArray.Length; index++)
            {
                // Если первое или последнее значение - не число
                double number;
                if ((index == 0 || index == inputExpressionArray.Length -1)
                    && !double.TryParse(inputExpressionArray[index], out number))
                {
                    return false;
                }

                // Если элемент не равен строке или числу
                if (!double.TryParse(inputExpressionArray[index], out number) &&
                    inputExpressionArray[index] != "+" &&
                    inputExpressionArray[index] != "-" &&
                    inputExpressionArray[index] != "*" &&
                    inputExpressionArray[index] != "/")
                {
                    return false;
                }
                // Если и предыдущий элемент - число, и текущий элемент - число
                if (!double.TryParse(inputExpressionArray[index], out number) && previous == 1)
                    return false;
                // Если и предыдущий элемент - опертор, и текущий элемент - оператор
                if ((inputExpressionArray[index] == "+" ||
                     inputExpressionArray[index] == "-" ||
                     inputExpressionArray[index] == "*" ||
                     inputExpressionArray[index] == "/") && previous == 0)
                    return false;
                
                if (inputExpressionArray[index] == "+" ||
                    inputExpressionArray[index] == "-" ||
                    inputExpressionArray[index] == "*" ||
                    inputExpressionArray[index] == "/")
                {
                    previous = 0;
                }
                else
                {
                    previous = 1;
                }
            }
            return true;
        }
        
        // Создает дерево выражений по введенному математическому выражению.
        private static Expression<Func<double>> CreateExpressionTree(string[] expressionArray)
        {
            var posfixExpression = infixToPostfix(expressionArray);
            var stack = new Stack<Expression>();
            for (var i = 0; i < posfixExpression.Count; i++)
            {
                var symbol = posfixExpression[i];
                // Если это символ...
                if (symbol != "+" || symbol != "-" || symbol != "*" || symbol != "/")
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
                // Иначе это число, которое нужно перевести в константу
                else
                {
                    stack.Push(Expression.Constant(double.Parse(symbol)));
                }
            }

            Expression result = stack.Pop();
            Expression<Func<double>> function
                = Expression.Lambda<Func<double>> (result);
            return function;
        }
        
        public static string CalculateExpression(string str)
        {
            if (!str.Contains(operators[0]) && !str.Contains(operators[1]) && !str.Contains(operators[2]) &&
                !str.Contains(operators[3])) return "Error";
            var expressionArray = str.Split(" ");
            if (!isUnputStringCorrect(expressionArray))
                return "Error";
            
            // строим дерево выражений...
            var expressionTree = CreateExpressionTree(expressionArray);
            var expressionVisitor = new ExpressionTreeVisitor();
            var result = expressionVisitor.GetAnswer(expressionTree);
            ////?????????return result.Body.ToString();
            throw new Exception("Недоделанный метод");
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

        // The main method that converts given infix expression  
        // to postfix expression.   
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
    }
}