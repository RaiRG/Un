using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hw8
{
    public class Calculator : ICalculator
    {
        public string CalculateExpression(string inputString)
        {
            // Проверяем, есть ли операторы в введенном выражении
            if (!inputString.Contains(operators[0]) && !inputString.Contains(operators[1]) &&
                !inputString.Contains(operators[2]) &&
                !inputString.Contains(operators[3])) return "Неверный формат!";
            var inputStringAsArray = inputString.Split(" ");
            if (!isInputStringCorrect(inputStringAsArray))
                return "Неверный формат!";
            var expressionTree = ExpressionTree.CreateExpressionTree(inputStringAsArray);
            var expressionVisitor = new ExpressionTreeVisitor();
            var result = expressionVisitor.GetAnswer(expressionTree);
            return result.Result.ToString();
        }

        private static readonly char[] operators = new char[]
        {
            '+', '-', '*', '/'
        };

        private static bool isInputStringCorrect(string[] inputExpressionArray)
        {
            var previous = -1; // 1 - число, 0 - оператор
            for (var index = 0; index < inputExpressionArray.Length; index++)
            {
                // Если первое или последнее значение - не число
                double number;
                if ((index == 0 || index == inputExpressionArray.Length - 1)
                    && (!double.TryParse(inputExpressionArray[index], out number) &&
                        inputExpressionArray[index] != "(" && inputExpressionArray[index] != ")"))
                {
                    return false;
                }

                // Если элемент не равен строке или числу
                if (!double.TryParse(inputExpressionArray[index], out number) &&
                    inputExpressionArray[index] != "+" &&
                    inputExpressionArray[index] != "-" &&
                    inputExpressionArray[index] != "*" &&
                    inputExpressionArray[index] != "/" &&
                    inputExpressionArray[index] != "(" &&
                    inputExpressionArray[index] != ")")
                {
                    return false;
                }

                // Если и предыдущий элемент - число, и текущий элемент - число
                if (double.TryParse(inputExpressionArray[index], out number) && previous == 1)
                    return false;
                // Если и предыдущий элемент - опертор, и текущий элемент - оператор
                if ((inputExpressionArray[index] == "+" ||
                     inputExpressionArray[index] == "-" ||
                     inputExpressionArray[index] == "*" ||
                     inputExpressionArray[index] == "/") && previous == 0)
                    return false;
                if (inputExpressionArray[index] == "(" || inputExpressionArray[index] == ")")
                    previous = -1;
                else if (inputExpressionArray[index] == "+" ||
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
    }
}