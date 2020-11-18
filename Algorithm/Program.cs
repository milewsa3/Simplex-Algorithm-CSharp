using System;
using static Algorithm.Result;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create data to compute
            ISimplexData data = new SimplexData();
            
            //Enter coefficients
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

            //Set up algorithm
            ISimplexAlgorithm simplexAlgorithm = new SimplexAlgorithm(Extreme.Minimum);
            simplexAlgorithm.EnterData(data);
            simplexAlgorithm.PrintTable();

            //Calculate
            Result result = simplexAlgorithm.Compute();

            if (result == Optimal)
            {
                Console.WriteLine("Result is optimal");
                double[] variables = simplexAlgorithm.GetComputedVariables();
                double minimum = simplexAlgorithm.GetExtreme();

                Console.WriteLine("Variables: " + variables);
                Console.WriteLine("Minimum: " + minimum);
            }
            else if (result == Unbounded)
            {
                Console.WriteLine("ERROR: Result is unbounded");
            }

        }
    }
}