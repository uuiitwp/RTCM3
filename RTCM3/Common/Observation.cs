using RTCM3.Common.Time;

namespace RTCM3.Common
{
    public class Observation
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


        public override string ToString()
        {
            return $"{GNSSSystemMethod.GetGNSSSystemChar(GNSSSystem)}{satId:00} {Frequency.GetRinexCode(GNSSSystem, sigId)} CNR={cnr:00}";
        }
    }
}
