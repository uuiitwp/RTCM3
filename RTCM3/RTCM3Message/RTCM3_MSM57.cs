using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public abstract class RTCM3_MSM57 : RTCM3_MSM
    {
        public double[] Range;
        public uint[] ex_sat_info;
        public double[] RangeM;
        public double[] RoughPhaseRangeRate;
        public double[] prv;
        public double[] cpv;
        public uint[] plock;
        public uint[] half;
        public uint[] cnr;
        public double[] FinePhaseRangeRate;
        public double[] PhaseRangeRate;
        public RTCM3_MSM57(ReadOnlySequence<byte> databody) : base(databody)
        {
            Range = new double[SatNumber];
            ex_sat_info = new uint[SatNumber];
            RangeM = new double[SatNumber];
            RoughPhaseRangeRate = new double[SatNumber];
            prv = new double[NCell];
            cpv = new double[NCell];
            plock = new uint[NCell];
            half = new uint[NCell];
            cnr = new uint[NCell];
            FinePhaseRangeRate = new double[NCell];
            PhaseRangeRate = new double[NCell];
            for (int j = 0; j < Range.Length; j++)
            {
                uint temp = BitOperation.GetBitsUint(databody, i, 8);
                Range[j] = temp == 0xFFu ? double.NaN : temp * Physics.RANGE_MS;
                i += 8;
            }

            for (int j = 0; j < ex_sat_info.Length; j++)
            {
                uint temp = BitOperation.GetBitsUint(databody, i, 4);
                ex_sat_info[j] = temp;
                i += 4;
            }

            for (int j = 0; j < RangeM.Length; j++)
            {
                RangeM[j] = BitOperation.GetBitsUint(databody, i, 10) * Common.Math.pow2_m10 * Physics.RANGE_MS;
                i += 10;
            }

            for (int j = 0; j < RoughPhaseRangeRate.Length; j++)
            {
                RoughPhaseRangeRate[j] = BitOperation.GetBitsInt(databody, i, 14);
                i += 14;
            }
        }

        protected void EncodeSatData(ref Span<byte> bytes)
        {
            throw new NotImplementedException();
        }

        public override Observation[] GetObservations()
        {
            int[] satIds = GetMaskedSatIds();
            int[] sigIds = GetMaskedSigIds();
            GNSSSystem sys = GetGNSSSystem();
            Observation[] obs = new Observation[NCell];
            Common.Time.GNSSTime time = Common.Time.GNSSTime.FromGNSSEpochTime(sys, GNSSEpochTime);
            for (int i = 0, k = 0; i < satIds.Length; i++)
            {
                for (int j = 0; j < sigIds.Length; j++)
                {
                    int index = i * sigIds.Length + j;
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
                            doppler = RoughPhaseRangeRate[i] + FinePhaseRangeRate[k],
                        };
                        k++;
                    }
                }
            }
            return obs;
        }
    }
}
