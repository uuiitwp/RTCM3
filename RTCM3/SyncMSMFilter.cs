using RTCM3.RTCM3Message;
using System.Buffers;

namespace RTCM3
{
    public class SyncMSMFilter
    {
        public static int InitLength { get; set; } = 8;
        private List<RTCM3> MSMs = new(InitLength);

        public SyncMSMFilter()
        {
        }
        public RTCM3[]? Filter(ref ReadOnlySequence<byte> buffer)
        {
            RTCM3? m = RTCM3.Filter(ref buffer);
            if (m != null && m.Databody is RTCM3_MSM msm)
            {
                MSMs.Add(m);
                if (msm.Sync == 0u)
                {
                    RTCM3[] result = MSMs.ToArray();
                    MSMs = new(InitLength);
                    return result;
                }
            }
            return null;
        }
    }
}
