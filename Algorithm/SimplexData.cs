using System;
using System.Runtime.InteropServices;

namespace Algorithm
{
    public class SimplexData
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
    
    public static class ArrayExt
    {
        public static T[] GetRow<T>(this T[,] array, int row)
        {
            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException("array");

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof(T) == typeof(bool))
                size = 1;
            else if (typeof(T) == typeof(char))
                size = 2;
            else
                size = Marshal.SizeOf<T>();

            Buffer.BlockCopy(array, row*cols*size, result, 0, cols*size);

            return result;
        }
    }
}