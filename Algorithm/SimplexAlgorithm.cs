namespace Algorithm
{
    public class SimplexAlgorithm
    {
        public Extreme Extreme { get; }
        public SimplexAlgorithm(Extreme extreme)
        {
            this.Extreme = extreme;
        }

        public void EnterData(SimplexData data)
        {
            
        }

        public void PrintTable()
        {
            
        }

        public Result Compute()
        {
            return Result.Unbounded;
        }

        public double[] GetComputedVariables()
        {
            return new double[] {};
        }

        public double GetExtreme()
        {
            return 0.0;
        }
    }
}