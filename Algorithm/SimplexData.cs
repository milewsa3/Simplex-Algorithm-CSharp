using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class SimplexData : ISimplexData
    {
        private double[] ObjectiveFunction { get; set; }
        private IList<Constraint> Constraints { get; set; }
        
        public SimplexData()
        {
            Constraints = new List<Constraint>();
        }

        public void AddObjectiveFunction(double[] objectiveFunction)
        {
            ObjectiveFunction = objectiveFunction;
        }

        public void AddConstraint(double[] constraint, ConstraintSign sign)
        {
            Constraints.Add(new Constraint(constraint, sign));
        }

        public void AddConstraints(double[,] constraints, ConstraintSign sign)
        {
            for (int i=0;i<constraints.GetLength(0);i++)
                AddConstraint(constraints.GetRow(i), sign);
            
        }
        
        public double[,] CreateSetOfData()
        {
            int numberOfConstraints = Constraints.Count;
            int numberOfConstraintsWithGreaterSign = numberOfConstraints +
                                                     Constraints.Count(x => x.Sign == ConstraintSign.GreaterOrEqual);
            int numberOfVariables = ObjectiveFunction.Length;
            double [,] data = new double[3 + numberOfConstraints, 3 + numberOfConstraintsWithGreaterSign + numberOfVariables];
            FillArray(data, 0);

            int coefficients = 0;
            for (; coefficients < numberOfVariables; coefficients++)
            {
                data[0, 2 + coefficients] = ObjectiveFunction[coefficients];
                data[1, 2 + coefficients] = coefficients + 1;
            }

            foreach (Constraint constraint in Constraints)
            {
                if (constraint.Sign == ConstraintSign.GreaterOrEqual)
                {
                    data[1, 2 + coefficients] = coefficients + 1;
                    data[0, 2 + coefficients++] = 0;
                    
                    data[1, 2 + coefficients] = coefficients + 1;
                    data[0, 2 + coefficients++] = double.PositiveInfinity;
                } 
                else if (constraint.Sign == ConstraintSign.Equal)
                {
                    data[1, 2 + coefficients] = coefficients + 1;
                    data[0, 2 + coefficients++] = double.PositiveInfinity;
                }
                else
                {
                    data[1, 2 + coefficients] = coefficients + 1;
                    data[0, 2 + coefficients++] = 0;
                }
            }

            for (int i = 0; i < numberOfConstraintsWithGreaterSign; i++)
            {
                data[2 + i, 0] = data[0, 2 + numberOfVariables + i];
                data[2 + i, 1] = data[1, 2 + numberOfVariables + i];
            }


            int slackVariableCounter = 0;
            for (int i = 0; i < Constraints.Count; i++)
            {
                Constraint cnstr = Constraints[i];
                double[] cdata = cnstr.Data;

                int j;
                for (j = 0; j < cdata.Length-1; j++)
                {
                    data[2 + i, 2 + j] = cdata[j];
                }

                data[2 + i, 2 + j + slackVariableCounter++] = 1;

                data[2 + i, data.GetLength(1) - 1] = cdata[^1];
            }


            return data;
        }

        private void FillArray(double[,] array, double value)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = 0;
                }
            }
        }
        
        public void PrintData()
        {
            Console.WriteLine("Generated set of data");
            double[,] data = CreateSetOfData();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                Console.Write("[\t");

                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Console.Write(data[i,j] + " \t ");
                }

                Console.WriteLine(']');
            }
        }
        
        public struct Constraint
        {
            public double[] Data { set; get; }
            public ConstraintSign Sign { set; get; }

            public Constraint(double[] constraint, ConstraintSign sign)
            {
                Data = constraint;
                this.Sign = sign;
            }
        }
    }
}