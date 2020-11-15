namespace Algorithm
{
    public interface ISimplexData
    {
        void AddObjectiveFunction(double[] objectiveFunction);
        void AddConstraint(double[] constraint, ConstraintSign sign);
        void AddConstraints(double[,] constraints, ConstraintSign sign);
        public double[,] CreateSetOfData();
        void PrintData();
        int GetNumberOfConstraints();
        int GetNumberOfVariables();
    }
}