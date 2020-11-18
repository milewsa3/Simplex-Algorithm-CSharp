using Algorithm;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Tests
{
    public class AlgorithmTest
    {
        private ISimplexAlgorithm algorithm;
        
        [SetUp]
        public void Setup()
        {
            algorithm = new SimplexAlgorithm(Extreme.Minimum);
        }

        [Test]
        public void CalculateExampleFromPlatform()
        {
            SimplexData data = SetupExampleData();
            algorithm.EnterData(data);
            algorithm.Compute();
            double[] actual = algorithm.GetComputedVariables();
            double[] expected = { 0, 450, 450, 0, 240, 450, 450, 0, 300 };

            AreEqual(expected, actual);
        }

        private SimplexData SetupExampleData()
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
    }
}