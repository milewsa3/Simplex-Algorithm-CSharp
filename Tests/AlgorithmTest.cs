using Algorithm;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Tests
{
    public class AlgorithmTest
    {
        private ISimplexAlgorithm _algorithm;

        [SetUp]
        public void Setup()
        {
            _algorithm = new SimplexAlgorithm(Extreme.Minimum);
        }

        [Test]
        public void Should_CalculateCorrectly_When_ExampleFromPlatformGiven()
        {
            //given
            SimplexData data = SetupDataFromPlatform();
            _algorithm.EnterData(data);

            //when
            _algorithm.Compute();

            //then
            ValidateExtremeAndVariables(
                _algorithm.GetExtreme(), _algorithm.GetComputedVariables(),
                189145.50, new double[] {0, 450, 450, 0, 240, 450, 450, 0, 300});
        }

        private SimplexData SetupDataFromPlatform()
        {
            SimplexData data = new SimplexData();
            double[] objectiveFunction = new[] {70.5, 70, 90.99, 100, 80, 70, 80, 90, 100};
            data.AddObjectiveFunction(objectiveFunction);

            double[,] constraints1 =
            {
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 800},
                {0, 1, 0, 0, 0, 0, 0, 0, 0, 600},
                {0, 0, 1, 0, 0, 0, 0, 0, 0, 750},
                {0, 0, 0, 1, 0, 0, 0, 0, 0, 900},
                {0, 0, 0, 0, 1, 0, 0, 0, 0, 600},
                {0, 0, 0, 0, 0, 1, 0, 0, 0, 450},
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 900},
                {0, 0, 0, 0, 0, 0, 0, 1, 0, 900},
                {0, 0, 0, 0, 0, 0, 0, 0, 1, 300},
                {1, 1, 1, 0, 0, 0, 0, 0, 0, 900},
                {0, 0, 0, 1, 1, 1, 0, 0, 0, 1300},
                {0, 0, 0, 0, 0, 0, 1, 1, 1, 1100},
            };

            double[,] constraints2 =
            {
                {1, 0, 0, 1, 0, 0, 1, 0, 0, 450},
                {0, 1, 0, 0, 1, 0, 0, 1, 0, 690},
                {0, 0, 1, 0, 0, 1, 0, 0, 1, 1200},
            };

            data.AddConstraints(constraints1, ConstraintSign.LessOrEqual);
            data.AddConstraints(constraints2, ConstraintSign.Equal);

            return data;
        }

        private void ValidateExtremeAndVariables(double extreme, double[] computedVariables,
            double actualExtreme, double[] actualVariables)
        {
            AreEqual(actualExtreme, extreme);
            AreEqual(actualVariables, computedVariables);
        }

        [Test]
        public void Should_CalculateCorrectly_When_TwoSamplePharmaciesAndProducersAreGiven()
        {
            //given
            ISimplexData data = SetupTwoPharmaciesAndProducers();
            _algorithm.EnterData(data);

            //when
            _algorithm.Compute();

            //than
            ValidateExtremeAndVariables(
                _algorithm.GetExtreme(), _algorithm.GetComputedVariables(),
                82425, new double[] {450, 450, 0, 240});
        }

        private ISimplexData SetupTwoPharmaciesAndProducers()
        {
            ISimplexData data = new SimplexData();

            double[] objectiveFunction = new[] {70.5, 70, 100, 80};
            data.AddObjectiveFunction(objectiveFunction);

            double[,] constarints1 =
            {
                {1, 0, 0, 0, 800},
                {0, 1, 0, 0, 600},
                {0, 0, 1, 0, 900},
                {0, 0, 0, 1, 600},
                {1, 1, 0, 0, 900},
                {0, 0, 1, 1, 1300},
            };

            double[,] constraints2 =
            {
                {1, 0, 1, 0, 450},
                {0, 1, 0, 1, 690}
            };

            data.AddConstraints(constarints1, ConstraintSign.LessOrEqual);
            data.AddConstraints(constraints2, ConstraintSign.Equal);

            return data;
        }
    }
}