using System;
using Algorithm;
using NUnit.Framework;

namespace Tests
{
    public class SimplexDataTests
    {
        private ISimplexData data;
        
        [SetUp]
        public void Setup()
        {
            data = new SimplexData();
        }

        [Test]
        public void Should_GenerateDataCorrectly_when_sampleDataIsGiven()
        {
            SetSampleObjectiveFunctionAndConstraints();

            double[,] expected =
            {
                {0, 0, 8, 10, 7, 0, 0, 0},
                {0, 0, 1, 2, 3, 4, 5, 0},
                {0, 4, 1, 3, 2, 1, 0, 10},
                {0, 5, 1, 5, 1, 0, 1, 8},
                {0, 0, 0, 0, 0, 0, 0, 0}
            };

            Assert.AreEqual(expected, data.CreateSetOfData());
        }

        private void SetSampleObjectiveFunctionAndConstraints()
        {
            double[] objectiveFunction = new[] {8.0, 10.0, 7.0};
            data.AddObjectiveFunction(objectiveFunction);

            double[,] constarints =
            {
                {1, 3, 2, 10},
                {1, 5, 1, 8}
            };

            data.AddConstraints(constarints, ConstraintSign.LessOrEqual);
        }
        
        [Test]
        public void should_throwArgumentException_when_wrongConstraintsDataGiven()
        {
            SetSampleObjectiveFunctionAndConstraints();
            data.AddConstraint(new double[] {0.1, 2}, ConstraintSign.Equal);
            
            Assert.Throws<ArgumentException>(() => data.CreateSetOfData());
        }
    }
}