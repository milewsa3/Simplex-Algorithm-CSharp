using System;
using Algorithm;
using NUnit.Framework;

namespace Tests
{
    public class SimplexDataTests
    {
        private ISimplexData _data;

        [SetUp]
        public void Setup()
        {
            _data = new SimplexData();
        }

        [Test]
        public void Should_GenerateDataCorrectly_When_SampleDataIsGiven()
        {
            //given
            GivenSampleObjectiveFunctionAndConstraints();

            //when

            //than
            ThanSampleDataShouldBeGeneratedCorrectly();
        }

        private void GivenSampleObjectiveFunctionAndConstraints()
        {
            double[] objectiveFunction = new[] {8.0, 10.0, 7.0};
            _data.AddObjectiveFunction(objectiveFunction);

            double[,] constarints =
            {
                {1, 3, 2, 10},
                {1, 5, 1, 8}
            };

            _data.AddConstraints(constarints, ConstraintSign.LessOrEqual);
        }

        private void ThanSampleDataShouldBeGeneratedCorrectly()
        {
            double[,] expected =
            {
                {0, 0, 8, 10, 7, 0, 0, 0},
                {0, 0, 1, 2, 3, 4, 5, 0},
                {0, 4, 1, 3, 2, 1, 0, 10},
                {0, 5, 1, 5, 1, 0, 1, 8},
                {0, 0, 0, 0, 0, 0, 0, 0}
            };

            Assert.AreEqual(expected, _data.CreateSetOfData());
        }

        [Test]
        public void Should_ThrowArgumentException_When_WrongConstraintsDataGiven()
        {
            //given
            GivenSampleObjectiveFunctionAndConstraints();

            //when
            _data.AddConstraint(new double[] {0.1, 2}, ConstraintSign.Equal);

            //than
            Assert.Throws<ArgumentException>(() => _data.CreateSetOfData());
        }
    }
}