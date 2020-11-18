using System;
using static Algorithm.Extreme;
using static Algorithm.Result;

namespace Algorithm
{
    
    
    
    public class SimplexAlgorithm : ISimplexAlgorithm
    {
        public Extreme Extreme { get; }
        public Boolean Computed = false;
        private int numberOfVariables;
        private int numberOfConstraints;
        private SimplexNumber[,] table;

        public SimplexAlgorithm(Extreme extreme)
        {
            Extreme = extreme;
        }

        public SimplexAlgorithm(double[,] data)
        {
            this.table = SimplexData.ConvertToSimplexNumbersArray(data);
        }

        
        
        public SimplexAlgorithm(double[,] data, Extreme extreme = Minimum)
        :this(extreme)
        {
            this.table = SimplexData.ConvertToSimplexNumbersArray(data);
        }

        public void EnterData(ISimplexData simplexData)
        {
            double[,] doubleData = simplexData.CreateSetOfData();
            table = SimplexData.ConvertToSimplexNumbersArray(doubleData);
            numberOfVariables = simplexData.GetNumberOfVariables();
            numberOfConstraints = simplexData.GetNumberOfConstraints();
            CalculateOptimizationIndicators();
        }

        private void CalculateOptimizationIndicators()
        {
            for (int i = 0; i < numberOfVariables + numberOfConstraints; i++)
            {
                SimplexNumber sum = new SimplexNumber();
                
                for (int j = 0; j < numberOfConstraints; j++)
                {
                    sum += table[2 + j, 2 + i] * table[2 + j, 0];
                }

                table[table.GetLength(0) - 1, 2 + i] = sum - table[0, 2 + i];
                Console.WriteLine(sum);
            }
        }

        public void PrintTable()
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                Console.Write("[" + "".PadRight(14,  ' '));

                for (int j = 0; j < table.GetLength(1); j++)
                {
                    Console.Write(table[i,j].ToString().PadRight(14,' ') + "");
                }

                Console.WriteLine(']');
            }

            Console.WriteLine('\n');
        }

        public Result Compute()
        {
            ValidateDataBeforeComputing();
            
            Result result = Compute(table);
            Computed = true;

            return result;
        }
        
        private void ValidateDataBeforeComputing()
        {
            if (table is null) 
                throw new ArgumentException("Data is not set");
        }
        
        private Result Compute(SimplexNumber[,] data)
        {
            if (TableIsOptimal(data))
                return Optimal;

            int pivotColumn = findPivotColumn(data);
            int pivotRow = findPivotRow(data);
            if (pivotRow is -1)
                return Unbounded;

            formNextTable(data);
            
            return Compute();
        }
        
        private bool TableIsOptimal(SimplexNumber[,] data)
        {
            if (Extreme == Maximum)
            {
                for (int i = 0; i < numberOfVariables + numberOfConstraints; i++)
                {
                    if (data[data.GetLength(0) - 1, 2 + i] <= 0)
                        return false;
                }

                return true;
            }
            else
            {
                for (int i = 0; i < numberOfVariables + numberOfConstraints; i++)
                {
                    if (data[data.GetLength(0) - 1, 2 + i] >= 0)
                        return false;
                }
            }

            return false;
        }

        private int findPivotColumn(SimplexNumber[,] data)
        {
            throw new NotImplementedException();
        }
        
        private int findPivotRow(SimplexNumber[,] data)
        {
            throw new NotImplementedException();
        }
        
        private void formNextTable(SimplexNumber[,] data)
        {
            throw new NotImplementedException();
        }

        public double[] GetComputedVariables()
        {
            CheckIfComputed();

            return new double[] {};
        }

        public double GetExtreme()
        {
            CheckIfComputed();

            return table.GetUpperBound(1);
        }

        private void CheckIfComputed()
        {
            if (!Computed)
                throw new Exception("Values are not computed yet");
        }
    }
}