using Algorithm;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Tests
{
    public class SimplexNumberTest
    {
        [Test]
        [TestCase(12)]
        [TestCase(-321)]
        [TestCase(412084127)]
        public void Should_GiveValidateDescription_When_ToStringIsCalled(int value)
        {
            //given
            SimplexNumber number = new SimplexNumber(value, 2);
            
            //when
            string actual = number.ToString();
            string expected = $"2M + {value}";
            
            //than
            AreEqual(expected, actual);
        }

        [Test]
        public void Should_PrintOnlyInfinity_When_StandardValueIsZero()
        {
            //given
            SimplexNumber number = new SimplexNumber(0, 12);
            
            //when
            string actual = number.ToString();
            string expected = "12M";
            
            //than
            AreEqual(expected, actual);
        }

        [Test]
        public void Should_AddInfinityAndStandardNumbers_When_ThereAreSimplexNumsWithBothValues()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(23, -2);
            SimplexNumber num2 = new SimplexNumber(-3, -1);
            
            //when
            SimplexNumber actual = num1 + num2;

            //than
            StandardValueAndInfinityValueAreEqual(actual, 20.0, -3);
        }

        private void StandardValueAndInfinityValueAreEqual(SimplexNumber num, double expectedStdVal, int expectedInfVal)
        {
            AreEqual(num.StandardValue, expectedStdVal);
            AreEqual(num.InfinityValue, expectedInfVal);
        }

        [Test]
        public void Should_EqualityCmpGiveTrue_WhenSimplexNumbsAreEqual()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(23, -2);
            SimplexNumber num2 = new SimplexNumber(23, -2);
            
            //than
            AreEqual(true, num1 == num2);
        }

        [Test]
        public void Should_GreaterCmpGiveTrue_WhenFirstNumbIsGreater()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(23, -2);
            SimplexNumber num2 = new SimplexNumber(23, -3);

            //than
            AreEqual(true, num1 > num2);
        }

        [Test]
        public void Should_LessOrEqualCmpGiveTrue_WhenFirstNumbIsLessOrEqual()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(15, 4);
            SimplexNumber num2 = new SimplexNumber(23, 4);

            //than
            AreEqual(true, num1 <= num2);
        }

        [Test]
        public void Should_BoxValueProperly_When_SampleDataIsGiven()
        {
            //when
            SimplexNumber actual = (SimplexNumber) 3.4;

            //than
            StandardValueAndInfinityValueAreEqual(actual, 3.4, 0);
        }

        [Test]
        public void Should_AddProperly_When_InfinityValueIsEmpty()
        {
            //given
            SimplexNumber actual = new SimplexNumber();
            SimplexNumber toAdd = new SimplexNumber(14);
            
            //when
            actual += toAdd;

            //than
            StandardValueAndInfinityValueAreEqual(actual, 14, 0);
        }

        [Test]
        public void Should_AddProperly_When_ThereIsNegativeInf()
        {
            //given
            SimplexNumber actual = new SimplexNumber(1);
            SimplexNumber toAdd = new SimplexNumber(14, -1);
            
            //when
            actual += toAdd;

            //than
            StandardValueAndInfinityValueAreEqual(actual, 15, -1);
        }

        [Test]
        public void Should_MultiplyProperly_When_StdValIsZero()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(0, 1);
            SimplexNumber num2 = new SimplexNumber(1);

            //when
            SimplexNumber actual = num1 * num2;
            
            //than
            StandardValueAndInfinityValueAreEqual(actual, 0, 1);
        }

        [Test]
        public void Should_DivideProperly_When_NoInfValIsGiven()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(20);
            SimplexNumber num2 = new SimplexNumber(8);

            //when
            SimplexNumber actual = num1 / num2;
            
            //than
            StandardValueAndInfinityValueAreEqual(actual, 2.5, 0);
        }

        [Test]
        public void Should_CompareProperly_When_NoInfValIsGiven()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(-60);
            SimplexNumber num2 = new SimplexNumber(-70);

            //than
            AreEqual(true, num1 > num2);
        }

        [Test]
        public void Should_CompareProperly_When_OneOfStdValIsZero()
        {
            //given
            SimplexNumber num1 = new SimplexNumber(60, -1);
            SimplexNumber num2 = new SimplexNumber(0);

            //than
            AreEqual(true, num1 < num2);
        }

        [Test]
        public void Should_CompareProperly_When_BoxingOneOfTheValues()
        {
            //given
            SimplexNumber num = new SimplexNumber(-70.5, 1);

            //than
            AreEqual(true, num > (SimplexNumber) 0);
        }

        [Test]
        public void Should_ResultOfCmpBeFalse_When_ComparingSimplexNumberWithZero()
        {
            //given
            SimplexNumber num = new SimplexNumber(-70.5, 1);

            //than
            //Result of comparison should be false,
            //because SimplexNumber is casted to double,
            //so the infinity value is lost
            AreEqual(false, num > 0); 
        }
    }
}