using Algorithm;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Tests
{
    public class SimplexNumberTest
    {
        [Test]
        public void ToStringTest()
        {
            SimplexNumber number = new SimplexNumber(10,2);
            string expected = "2M + (10)";
            string actual = number.ToString();
            
            AreEqual(expected, actual);
        }

        [Test]
        public void AddingStandardValueAndInfinity()
        {
            SimplexNumber num1 = new SimplexNumber(23, -2);
            SimplexNumber num2 = new SimplexNumber(-3, -1);
            SimplexNumber actual = num1 + num2;
            double expectedStandard = 20.0;
            int expectedInfinity = -3;

            AreEqual(expectedStandard, actual.StandardValue);
            AreEqual(expectedInfinity, actual.InfinityValue);
        }

        [Test]
        public void EqualComparison()
        {
            SimplexNumber num1 = new SimplexNumber(23, -2);
            SimplexNumber num2 = new SimplexNumber(23, -2);
            
            AreEqual(true, num1 == num2);
        }

        [Test]
        public void GreaterComparison()
        {
            SimplexNumber num1 = new SimplexNumber(23, -2);
            SimplexNumber num2 = new SimplexNumber(23, -3);
            
            AreEqual(true, num1 > num2);
        }

        [Test] 
        public void LessOrEqualComparison()
        {
            SimplexNumber num1 = new SimplexNumber(15, 4);
            SimplexNumber num2 = new SimplexNumber(23, 4);
            
            AreEqual(true, num1 <= num2);
        }

        [Test]
        public void BoxingTest()
        {
            double sample = 3.4;
            SimplexNumber actual = (SimplexNumber) sample;
            
            AreEqual(3.4, actual.StandardValue);
            AreEqual(0, actual.InfinityValue);
        }
    }
}