using System;
using static Algorithm.Result;

namespace Algorithm
{
    public class SimplexAlgorithm : ISimplexAlgorithm
    {
        private bool Computed { set; get; }
        private SimplexNumber[,] _table;
        private int _numberOfVariables;
        private int _numberOfConstraints;


        public SimplexAlgorithm() {}

        public SimplexAlgorithm(ISimplexData data)
            => _table = data.CreateSetOfSimplexData();

        public void SetData(ISimplexData data)
        {
            _table = data.CreateSetOfSimplexData();

            _numberOfVariables = data.GetNumberOfVariables();
            _numberOfConstraints = data.GetNumberOfConstraints();

            CalculateOptimizationIndicators(_table);
        }

        private void CalculateOptimizationIndicators(SimplexNumber[,] table)
        {
            for (int i = 0; i < _numberOfVariables + _numberOfConstraints + 1; i++)
            {
                SimplexNumber sum = new SimplexNumber();

                for (int j = 0; j < _numberOfConstraints; j++)
                {
                    sum += table[2 + j, 2 + i] * table[2 + j, 0];
                }

                table[table.GetLength(0) - 1, 2 + i] = sum - table[0, 2 + i];
            }
        }

        public Result Compute()
        {
            ValidateDataBeforeComputing();

            Result result = Compute(_table);
            Computed = true;

            return result;
        }

        private void ValidateDataBeforeComputing()
        {
            if (_table is null)
                throw new ArgumentException("Data is not set");
        }

        private Result Compute(SimplexNumber[,] data)
        {
            if (TableIsOptimal(data))
            {
                _table = data;
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
            for (int i = 0; i < _numberOfVariables + _numberOfConstraints; i++)
            {
                if (data[data.GetLength(0) - 1, 2 + i] > (SimplexNumber) 0)
                {
                    return false;
                }
            }

            return true;
        }

        private int FindPivotColumn(SimplexNumber[,] data)
        {
            int maxIndx = 0;

            for (int i = 1; i < _numberOfVariables + _numberOfConstraints; i++)
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

            for (int i = 0; i < _numberOfConstraints; i++)
            {
                if (data[2 + i, pivotCol] == 0)
                    continue;

                if (rowIndex == -1 && data[2 + i, n - 1] / data[2 + i, pivotCol] > 0)
                {
                    rowIndex = i;
                    value = data[2 + i, n - 1] / data[2 + i, pivotCol];
                    continue;
                }

                if (((data[2 + i, n - 1] / data[2 + i, pivotCol]) < value) &&
                    (data[2 + i, n - 1] / data[2 + i, pivotCol] > 0))
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

            for (int i = 0; i < _numberOfConstraints; i++)
            {
                newTable[2 + i, 0] = data[2 + i, 0];
                newTable[2 + i, 1] = data[2 + i, 1];
            }

            newTable[pivotRow, 0] = newTable[0, pivotCol];
            newTable[pivotRow, 1] = newTable[1, pivotCol];


            SimplexNumber pivotCenter = data[pivotRow, pivotCol];
            for (int i = 0; i < _numberOfVariables + _numberOfConstraints + 1; i++)
            {
                newTable[pivotRow, 2 + i] = data[pivotRow, 2 + i] / pivotCenter;
            }

            for (int i = 0; i < _numberOfConstraints; i++)
            {
                if (i + 2 == pivotRow)
                    continue;

                if (data[2 + i, pivotCol] == 0)
                {
                    for (int j = 0; j < _numberOfVariables + _numberOfConstraints + 1; j++)
                    {
                        newTable[2 + i, 2 + j] = data[2 + i, 2 + j];
                    }

                    continue;
                }


                SimplexNumber delta = (SimplexNumber) (-1) * data[2 + i, pivotCol];
                for (int j = 0; j < _numberOfVariables + _numberOfConstraints + 1; j++)
                {
                    newTable[2 + i, 2 + j] = data[2 + i, 2 + j] + delta * data[pivotRow, 2 + j];
                }
            }

            CalculateOptimizationIndicators(newTable);

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
            double[] variables = new double[_numberOfVariables];

            for (int i = 0; i < _numberOfVariables; i++)
            {
                bool wasOne = false;
                int oneIndx = -1;

                for (int j = 0; j < _numberOfConstraints; j++)
                {
                    if (Math.Abs(_table[2 + j, 2 + i] - 1) > 0.000001 && _table[2 + j, 2 + i] != 0)
                        break;

                    if (wasOne && Math.Abs(_table[2 + j, 2 + i] - 1) < 0.000001)
                    {
                        wasOne = false;
                        break;
                    }

                    if (Math.Abs(_table[2 + j, 2 + i] - 1) < 0.000001)
                    {
                        wasOne = true;
                        oneIndx = j;
                    }
                }

                if (wasOne)
                {
                    variables[i] = _table[oneIndx + 2, _table.GetLength(1) - 1];
                }
                else
                {
                    variables[i] = 0;
                }
            }

            return variables;
        }

        public double GetCalculatedExtreme()
        {
            CheckIfComputed();

            int n = _table.GetLength(0);
            int m = _table.GetLength(1);

            return _table[n - 1, m - 1];
        }

        private void CheckIfComputed()
        {
            if (!Computed)
                throw new Exception("Values are not computed yet");
        }

        private void PrintTable() => PrintTable(_table);

        private void PrintTable(SimplexNumber[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                Console.Write("[" + "".PadRight(14, ' '));

                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Console.Write(data[i, j].ToString().PadRight(14, ' ') + "");
                }

                Console.WriteLine(']');
            }

            Console.WriteLine('\n');
        }
    }
}