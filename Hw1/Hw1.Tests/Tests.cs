using System;
using Hw1;
using NUnit.Framework;

namespace Hw1.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Calculate_2Plus50_52Returned()
        {
            Assert.AreEqual(52, Calculator.Calculate("+", 2, 50));
        }

        [Test]
        public void Calculate_2Times8_16Returned()
        {
            Assert.AreEqual(16, Calculator.Calculate("*", 2, 8));
        }

        [Test]
        public void Calculate_20Minus16_4Returned()
        {
            Assert.AreEqual(4, Calculator.Calculate("-", 20, 16));
        }

        [Test]
        public void Calculate_10Divided2_5Returned()
        {
            Assert.AreEqual(5, Calculator.Calculate("/", 10, 2));
        }

        [Test]
        public void Calculate_100Divided0_DivideByZeroExceptionReturned()
        {
            Assert.Throws<DivideByZeroException>(new TestDelegate(() => ThrowMethodWithDividedByZero()));
        }

        private void ThrowMethodWithDividedByZero()
        {
            Calculator.Calculate("/", 100, 0);
        }

        [Test]
        public void Calculate_InvalidСharacter_NotSupportedException()
        {
            Assert.Throws(typeof(NotSupportedException), new TestDelegate(() => ThrowMethodWithInvalicCharacter()));
        }

        private void ThrowMethodWithInvalicCharacter()
        {
            Calculator.Calculate(">", 7, 8);
        }
    }
}