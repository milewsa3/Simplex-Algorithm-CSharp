namespace Algorithm
{
    public interface ISimplexAlgorithm
    {
        void EnterData(ISimplexData data);
        void PrintTable();
        Result Compute();
        double[] GetComputedVariables();
        double GetExtreme();
    }
}