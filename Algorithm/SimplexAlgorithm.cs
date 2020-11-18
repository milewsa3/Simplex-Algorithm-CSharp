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
            for (int i = 0; i < numberOfVariables + numberOfConstraints + 1; i++)
            {
                SimplexNumber sum = new SimplexNumber();
                
                for (int j = 0; j < numberOfConstraints; j++)
                {
                    sum += table[2 + j, 2 + i] * table[2 + j, 0];
                }

                table[table.GetLength(0) - 1, 2 + i] = sum - table[0, 2 + i];
            }
        }

        public void PrintTable() => PrintTable(table);

        private void PrintTable(SimplexNumber[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                Console.Write("[" + "".PadRight(14,  ' '));

                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Console.Write(data[i,j].ToString().PadRight(14,' ') + "");
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
            {
                table = data;
                return Optimal;
            }

            int pivotColumn = FindPivotColumn(data);
            int pivotRow = FindPivotRow(data, pivotColumn);
            if (pivotRow == -1)
                return Unbounded;

            SimplexNumber[,] newTable = FormNextTable(data, pivotColumn, pivotRow);
            
            return Compute(newTable);
        }

        private bool TableIsOptimal(SimplexNumber[,] data)
        {
            for (int i = 0; i < numberOfVariables + numberOfConstraints; i++)
            {
                if (data[data.GetLength(0) - 1, 2 + i] > (SimplexNumber)0)
                {
                    return false;
                }
            }

            return true;
        }

        private int FindPivotColumn(SimplexNumber[,] data)
        {
            int maxIndx = 0;
            
            for (int i = 1; i < numberOfVariables + numberOfConstraints; i++)
            {
                if (data[data.GetLength(0) - 1, 2 + i] > data[data.GetLength(0) - 1, 2 + maxIndx])
                    maxIndx = i;
            }

            return maxIndx + 2;
        }
        
        private int FindPivotRow(SimplexNumber[,] data, int pivotCol)
        {
            int rowIndex = -1;
            int n = data.GetLength(1);
            SimplexNumber value = new SimplexNumber(0);
            
            for (int i = 0; i < numberOfConstraints; i++)
            {
                if (data[2+i, pivotCol] == 0 )
                    continue;

                if (rowIndex == -1 && data[2 + i, n - 1] / data[2 + i, pivotCol] > 0)
                {
                    rowIndex = i;
                    value = data[2 + i, n - 1] / data[2 + i, pivotCol];
                    continue;
                }

                if (((data[2 + i, n - 1] / data[2 + i, pivotCol]) < value) && (data[2 + i, n - 1] / data[2 + i, pivotCol] > 0))
                {
                    rowIndex = i;
                    value = data[2 + i, n - 1] / data[2 + i, pivotCol];
                }
            }

            return rowIndex + 2;
        }
        
        private SimplexNumber[,] FormNextTable(SimplexNumber[,] data, int pivotCol, int pivotRow)
        {
            int n = data.GetLength(0);
            int m = data.GetLength(1);

            SimplexNumber[,] newTable = new SimplexNumber[n, m];

            for (int i = 0; i < m; i++)
            {
                newTable[0, i] = data[0, i];
                newTable[1, i] = data[1, i];
            }

            for (int i = 0; i < numberOfConstraints; i++)
            {
                newTable[2 + i, 0] = data[2 + i, 0];
                newTable[2 + i, 1] = data[2 + i, 1];
            }

            newTable[pivotRow, 0] = newTable[0, pivotCol];
            newTable[pivotRow, 1] = newTable[1, pivotCol];


            SimplexNumber pivotCenter = data[pivotRow, pivotCol];
            for (int i = 0; i < numberOfVariables + numberOfConstraints + 1; i++)
            {
                newTable[pivotRow, 2 + i] = data[pivotRow, 2 + i] / pivotCenter;
            }

            for (int i = 0; i < numberOfConstraints; i++)
            {
                if (i + 2 == pivotRow)
                    continue;

                if (data[2 + i, pivotCol] == 0)
                {
                    for (int j = 0; j < numberOfVariables + numberOfConstraints + 1; j++)
                    {
                        newTable[2 + i, 2 + j] = data[2 + i, 2 + j];
                    }
                    
                    continue;
                }


                SimplexNumber delta = (SimplexNumber)(-1) * data[2 + i, pivotCol];
                for (int j = 0; j < numberOfVariables + numberOfConstraints + 1; j++)
                {
                    newTable[2 + i, 2 + j] = data[2 + i, 2 + j] + delta * data[pivotRow, 2 + j];
                }
            }
            
            for (int i = 0; i < numberOfVariables + numberOfConstraints + 1; i++)
            {
                SimplexNumber sum = new SimplexNumber();
                
                for (int j = 0; j < numberOfConstraints; j++)
                {
                    sum += newTable[2 + j, 2 + i] * newTable[2 + j, 0];
                }

                newTable[newTable.GetLength(0) - 1, 2 + i] = sum - newTable[0, 2 + i];
            }

            return newTable;
        }

        public double[] GetComputedVariables()
        {
            CheckIfComputed();

            double[] variables = ComputeVariables();

            return variables;
        }

        private double[] ComputeVariables()
        {
            double[] variables = new double[numberOfVariables];

            for (int i = 0; i < numberOfVariables; i++)
            {
                bool wasOne = false;
                int oneIndx = -1;
                
                for (int j = 0; j < numberOfConstraints; j++)
                {
                    if (table[2 + j, 2 + i] != 1 && table[2 + j, 2 + i] != 0)
                        break;

                    if (wasOne && table[2 + j, 2 + i] == 1)
                    {
                        wasOne = false;
                        break;
                    }
                    
                    if (table[2 + j, 2 + i] == 1)
                    {
                        wasOne = true;
                        oneIndx = j;
                    }
                }

                if (wasOne)
                {
                    variables[i] = table[oneIndx+2,table.GetLength(1)-1];
                }
                else
                {
                    variables[i] = 0;
                }
            }

            return variables;
        }

        public double GetExtreme()
        {
            CheckIfComputed();

            int n = table.GetLength(0);
            int m = table.GetLength(1);

            return table[n - 1, m - 1];
        }

        private void CheckIfComputed()
        {
            if (!Computed)
                throw new Exception("Values are not computed yet");
        }
    }
}