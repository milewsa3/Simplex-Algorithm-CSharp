using System;
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

        public SimplexData(double[] objectiveFunction, IList<Constraint> constraints)
        {
            ObjectiveFunction = objectiveFunction;
            Constraints = constraints;
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
            for (int i = 0; i < constraints.GetLength(0); i++)
                AddConstraint(constraints.GetRow(i), sign);
        }

        public double[,] CreateSetOfData()
        {
            ValidateData();

            int numberOfConstraints = GetNumberOfConstraints();

            int numberOfVariables = GetNumberOfVariables();
            double[,] data = new double[3 + numberOfConstraints,
                3 + numberOfConstraints + numberOfVariables];
            data.Fill(0);

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

            for (int i = 0; i < numberOfConstraints; i++)
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
                for (j = 0; j < cdata.Length - 1; j++)
                {
                    data[2 + i, 2 + j] = cdata[j];
                }

                data[2 + i, 2 + j + slackVariableCounter++] = 1;

                data[2 + i, data.GetLength(1) - 1] = cdata[^1];
            }


            return data;
        }

        private void ValidateData()
        {
            if (ObjectiveFunction is null)
                throw new ArgumentException("Objective function must be set");

            if (Constraints.Count == 0)
                throw new ArgumentException("No Constraints added!");

            int numberOfVariables = ObjectiveFunction.Length;

            foreach (Constraint constraint in Constraints)
            {
                double[] data = constraint.Data;
                if (numberOfVariables + 1 != data.Length)
                    throw new ArgumentException($"Constrain {constraint} is not proper. There should be: " +
                                                $"{numberOfVariables + 1} variables");
                
                if (constraint.Sign == ConstraintSign.GreaterOrEqual)
                    throw new NotSupportedException();
            }
        }

        public SimplexNumber[,] CreateSetOfSimplexData()
        {
            double[,] data = CreateSetOfData();
            return ConvertToSimplexNumberArray(data);
        }

        private SimplexNumber[,] ConvertToSimplexNumberArray(double[,] data)
        {
            SimplexNumber[,] result = new SimplexNumber[data.GetLength(0), data.GetLength(1)];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = (SimplexNumber) data[i, j];
                }
            }

            return result;
        }

        public int GetNumberOfVariables()
        {
            return ObjectiveFunction.Length;
        }

        public int GetNumberOfConstraints()
        {
            return Constraints.Count;
        }

        public void PrintData() => CreateSetOfData().Print();

        public struct Constraint
        {
            public double[] Data { private set; get; }
            public ConstraintSign Sign { private set; get; }

            public Constraint(double[] constraint, ConstraintSign sign)
            {
                Data = constraint;
                this.Sign = sign;
            }

            public override string ToString() => Data.ToString();
        }
    }
}