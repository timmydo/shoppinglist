namespace backend.Interfaces.Infrastructure
{
    public interface IMetricTracker
    {
        void Track(double amt);
    }
}