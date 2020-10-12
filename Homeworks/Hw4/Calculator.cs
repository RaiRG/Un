using System;
using System.Collections.Generic;
using System.Text;

namespace Hw4
{
    public class Calculator
    {
        public static string Calculate(string @operator, int a, int b)
        {
            var result = @operator switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0 ? throw new DivideByZeroException() : a / b,
                _ => throw new NotSupportedException()
            };
            return result.ToString();
        }

        private static readonly char[] operators = new char[]
        {
            '+', '-', '*', '/'
        };

        public static string Calc(string str)
        {
            if (!str.Contains(operators[0]) && !str.Contains(operators[1]) && !str.Contains(operators[2]) &&
                !str.Contains(operators[3])) return "Error";
            var position = str.IndexOfAny(operators);
            var fi = str.Substring(0, position);
            var first = int.Parse(fi);
            var second = int.Parse(str.Substring(position+1));
            return Calculate(str[position].ToString(), first, second);

        }
    }
}