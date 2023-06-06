using RTCM3.Common;
using RTCM3.Common.Frequency;

namespace RTCM3.RTCM3Message
{
    public abstract class RTCM3_MSM46 : RTCM3_MSM
    {
        public double[] Range;
        public double[] RangeM;
        public double[] prv;
        public double[] cpv;
        public uint[] plock;
        public uint[] half;
        public uint[] cnr;
        public RTCM3_MSM46(ReadOnlySpan<byte> databody) : base(databody)
        {
            Range = new double[SatNumber];
            RangeM = new double[SatNumber];
            prv = new double[NCell];
            cpv = new double[NCell];
            plock = new uint[NCell];
            half = new uint[NCell];
            cnr = new uint[NCell];
            for (int j = 0; j < Range.Length; j++)
            {
                uint temp = BitOperation.GetBitsUint(databody, i, 8);
                Range[j] = temp == 0xFFu ? double.NaN : temp * Physics.RANGE_MS;
                i += 8;
            }

            for (int j = 0; j < RangeM.Length; j++)
            {
                RangeM[j] = BitOperation.GetBitsUint(databody, i, 10) * Common.Math.pow2_m10 * Physics.RANGE_MS;
                i += 10;
            }
        }

        protected void EncodeSatData(ref Span<byte> bytes)
        {
            int i = 24 + 169 + Cell.Length;
            for (int j = 0; j < Range.Length; j++)
            {
                uint temp = double.IsNaN(Range[j]) ? 0xFFu : (uint)RoundToLong(Range[j] / Physics.RANGE_MS);
                BitOperation.SetBitsUint(ref bytes, i, 8, temp);
                i += 8;
            }
            for (int j = 0; j < RangeM.Length; j++)
            {
                BitOperation.SetBitsUint(ref bytes, i, 10, (uint)RoundToLong(RangeM[j] / Common.Math.pow2_m10 / Physics.RANGE_MS));
                i += 10;
            }
        }

        public override Observation[] GetObservations()
        {
            int[] satIds = GetMaskedSatIds();
            int[] sigIds = GetMaskedSigIds();
            GNSSSystem sys = GetGNSSSystem();
            Observation[] obs = new Observation[NCell];
            Common.Time.GNSSTime time = Common.Time.GNSSTime.FromTow(sys, GNSStime / 1000.0);
            for (int i = 0, k = 0; i < satIds.Length; i++)
            {
                for (int j = 0; j < sigIds.Length; j++)
                {
                    int index = (i * sigIds.Length + j);
                    if (Cell[index])
                    {
                        double freq = Frequency.GetFrequency(sys, satIds[i], sigIds[j]);
                        obs[k] = new Observation()
                        {
                            GNSSSystem = sys,
                            satId = satIds[i],
                            sigId = sigIds[j],
                            cnr = cnr[k],
                            plock = plock[k],
                            half = half[k],
                            pseudoRange = Range[i] + RangeM[i] + prv[k],
                            carrierPhase = (Range[i] + RangeM[i] + cpv[k]) * freq / Physics.CLIGHT,
                            GNSSTime = time,
                        };
                        k++;
                    }
                }
            }
            return obs;
        }
    }
}
