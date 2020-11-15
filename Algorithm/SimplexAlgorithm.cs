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
        private double[,] _data;

        public SimplexAlgorithm(Extreme extreme)
        {
            Extreme = extreme;
        }

        public SimplexAlgorithm(double[,] data)
        {
            this._data = data;
        }
        
        public SimplexAlgorithm(double[,] data, Extreme extreme = Minimum)
        :this(extreme)
        {
            this._data = data;
        }

        public void EnterData(ISimplexData simplexData)
        {
            _data = simplexData.CreateSetOfData();
            numberOfVariables = simplexData.GetNumberOfVariables();
            numberOfConstraints = simplexData.GetNumberOfConstraints();
        }

        public void PrintTable()
        {
            SimplexData.PrintData(_data);
        }

        public Result Compute()
        {
            ValidateDataBeforeComputing();
            
            Result result = Compute(_data);
            Computed = true;

            return result;
        }
        
        private void ValidateDataBeforeComputing()
        {
            if (_data is null) 
                throw new ArgumentException("Data is not set");
        }
        
        private Result Compute(double[,] data)
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
        
        private bool TableIsOptimal(double[,] data)
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

        private int findPivotColumn(double[,] data)
        {
            throw new NotImplementedException();
        }
        
        private int findPivotRow(double[,] data)
        {
            throw new NotImplementedException();
        }
        
        private void formNextTable(double[,] data)
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

            return _data.GetUpperBound(1);
        }

        private void CheckIfComputed()
        {
            if (!Computed)
                throw new Exception("Values are not computed yet");
        }
    }
}