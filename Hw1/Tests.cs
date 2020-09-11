using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Hw1
{
    [TestFixture]
    class Tests
    {
        [Test]
        public void Sum()
        {
            Assert.AreEqual(52, Calculator.Calculate("+", 2, 50));
        }
        [Test]
        public void Multiply()
        {
            Assert.AreEqual(16, Calculator.Calculate("*", 2, 8));
        }
        [Test]
        public void Difference()
        {
            Assert.AreEqual(4, Calculator.Calculate("-", 20, 16));
        }
        [Test]
        public void Division()
        {
            Assert.AreEqual(5, Calculator.Calculate("/", 10, 2));
        }
    }
}
