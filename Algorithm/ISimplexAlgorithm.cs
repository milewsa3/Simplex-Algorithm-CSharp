namespace Algorithm
{
    public interface ISimplexAlgorithm
    {
        void SetData(ISimplexData data);
        Result Compute();
        double[] GetComputedVariables();
        double GetCalculatedExtreme();
    }
}