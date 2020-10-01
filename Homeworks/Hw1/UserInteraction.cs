using System;
using System.Collections.Generic;
using System.Text;

namespace Hw1
{
    class UserInteraction
    {
        public static int GetNumber()
        {
            return int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
        }
    }
}