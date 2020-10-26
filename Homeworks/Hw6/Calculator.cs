using System;
using System.Collections.Generic;
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

        public static string CalculateExpression(string str)
        {
            if (!str.Contains(operators[0]) && !str.Contains(operators[1]) && !str.Contains(operators[2]) &&
                !str.Contains(operators[3])) return "Error";
            var expressionArray = str.Split(" ");
            // Следующий цикл проверяет, корректное ли выражение.
            for (var index = 0; index < expressionArray.Length; index++)
            {
                // Если первый символ - не число
                double number;
                if (index == 0 && !double.TryParse(expressionArray[index], out number))
                {
                    return "Error";
                }
                // Если элемент не равен строке или числу
                if (!double.TryParse(expressionArray[index], out number) &&
                    expressionArray[index] != "+" &&
                    expressionArray[index] != "-" &&
                    expressionArray[index] != "*" &&
                    expressionArray[index] != "/")
                {
                    return "Error";
                }
                // Если предыдущее - число, и следующее - число, аналогично с операторами.
                // ...

            }
            // Оказались здесь, если все ок (строка прошла проверку)
            var posfixExpression = infixToPostfix(expressionArray);

            var temporaryStackExpression = new Stack<Expression>();
            for (var i = 0; i < posfixExpression.Count; i++)
            {
                
            }

            var expressionTreeBuilder = new CustomExpressionTreeVisitor();
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