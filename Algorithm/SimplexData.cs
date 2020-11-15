using System;
using System.Runtime.InteropServices;

namespace Algorithm
{
    public class SimplexData : ISimplexData
    {
        public SimplexData()
        {
            
        }

        public void AddObjectiveFunction(double[] targetFunction)
        {
            
        }

        public void AddConstraint(double[] constraint, ConstraintSign sign)
        {
            
        }

        public void AddConstraints(double[,] constraints, ConstraintSign sign)
        {
            for (int i=0;i<constraints.GetLength(0);i++)
                AddConstraint(constraints.GetRow(i), sign);
                
        }
    }
}