using RTCM3.Common;

namespace RTCM3
{
    public struct Observation
    {
        public GNSSSystem GNSSSystem;
        public int satId;
        public int sigId;
        public uint cnr;
        public uint plock;
        public uint half;
        public double pseudoRange;
        public double carrierPhase;
        public double doppler;
    }
}
