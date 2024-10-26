namespace RTCM3.Common
{
    public class UnsupportedGNSSSystem(GNSSSystem GNSSSystem) : Exception($"{GNSSSystem} is not supported")
    {
    }
}
