namespace Algorithm
{
    public interface ISimplexData
    {
        void AddObjectiveFunction(double[] objectiveFunction);
        void AddConstraint(double[] constraint, ConstraintSign sign);
        void AddConstraints(double[,] constraints, ConstraintSign sign);
        public double[,] CreateSetOfData();
        public SimplexNumber[,] CreateSetOfSimplexData();
        int GetNumberOfVariables();
        int GetNumberOfConstraints();
        void PrintData();
    }
}