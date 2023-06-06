using RTCM3.Common;
using RTCM3.Common.Frequency;
using RTCM3.Common.Time;

namespace RTCM3
{
    public struct Observation
    {
        public GNSSSystem GNSSSystem;
        public GNSSTime GNSSTime;
        public int satId;
        public int sigId;
        public uint cnr;
        public uint plock;
        public uint half;
        public double pseudoRange;
        public double carrierPhase;
        public double doppler;


        public override readonly string ToString()
        {
            return $"{GNSSSystemMethod.GetGNSSSystemChar(GNSSSystem)}{satId:00} {Frequency.GetRinexCode(GNSSSystem, sigId)} CNR={cnr:00}";
        }
    }
}
