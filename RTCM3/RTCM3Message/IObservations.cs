using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public interface IObservations
    {
        public abstract Observation[] GetObservations();
    }
}
