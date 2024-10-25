using System.IO.Pipelines;

namespace TestRTCM3
{
    [TestClass]
    public class MockTest
    {
        public static FileInfo[] GetFiles()
        {
            DirectoryInfo directoryInfo = new("RTCM3_bin");
            return directoryInfo.GetFiles("*.rtcm3", SearchOption.AllDirectories);
        }
        [TestMethod]
        public void DecodeEncodeRTCM3()
        {
            Span<byte> span = stackalloc byte[2048];
            foreach (FileInfo file in GetFiles())
            {
                Console.WriteLine($"file name: {file.Name}");
                using FileStream fs = File.OpenRead(file.FullName);
                PipeReader pipeReader = PipeReader.Create(fs);
                while (true)
                {
                    ReadResult rs = pipeReader.ReadAsync().AsTask().Result;
                    ReadOnlySequence<byte> buffer = rs.Buffer;
                    ReadOnlySequence<byte> read = buffer.Slice(0);

                    RTCM3.RTCM3? message = RTCM3.RTCM3.Filter(ref buffer);

                    if (message != null)
                    {
                        try
                        {
                            if (message.Databody != null)
                            {
                                int len = message.Databody.Encode(ref span);
                                long start = read.GetOffset(buffer.Start) - read.GetOffset(read.Start);
                                byte[] a = span[..len].ToArray();
                                byte[] b = read.Slice(start - len, len).ToArray();
                                if (!a.SequenceEqual(b))
                                {
                                    Console.WriteLine(BitConverter.ToString(a).Replace('-', ' '));
                                    Console.WriteLine(BitConverter.ToString(b).Replace('-', ' '));
                                    Assert.Fail("Sequences are not equal.");
                                }
                            }
                        }
                        catch (NotImplementedException)
                        {
                            Console.WriteLine($"RTCM_{message.MessageType} Encode method is not implemented.");
                        }
                    }
                    pipeReader.AdvanceTo(buffer.Start, buffer.End);
                    if (rs.IsCompleted)
                    {
                        break;
                    }
                }
            }

        }


        [TestMethod]
        public void DecodeSyncMSM()
        {
            foreach (FileInfo file in GetFiles())
            {
                SyncMSMsFilter Filter = new();
                byte[] bs = File.ReadAllBytes(file.FullName);
                ReadOnlySequence<byte> rs = new(bs);
                long rs_remain = 0;
                while (true)
                {
                    if (rs.IsEmpty)
                    {
                        break;
                    }
                    if (rs_remain == rs.Length)
                    {
                        break;
                    }
                    rs_remain = rs.Length;
                    RTCM3.RTCM3[]? msms = Filter.Filter(ref rs);
                    if (msms is not null)
                    {
                        Assert.AreEqual((msms.Last().Databody as RTCM3_MSM)?.Sync, 0u);
                        foreach (RTCM3.RTCM3 msm in msms[..^1])
                        {
                            Assert.AreEqual((msm.Databody as RTCM3_MSM)?.Sync, 1u);
                        }
                        List<GNSSTime> result = [];
                        foreach (RTCM3.RTCM3 msm in msms)
                        {
                            RTCM3_MSM? m = msm.Databody as RTCM3_MSM;
                            try
                            {
                                Observation[]? o = m?.GetObservations();
                                IEnumerable<GNSSTime>? t = o?.Select(x => x.GNSSTime);
                                if (t != null)
                                {
                                    result.AddRange(t);
                                }
                            }
                            catch (UnsupportedGNSSSystem)
                            {

                            }
                        }
                        bool f = result.All(x => x.Equals(result.First()));
                        Assert.IsTrue(f);
                    }
                }
            }
        }


        [TestMethod]
        public void TestLeepsecond()
        {
            DateTime t = new(2022, 1, 1);
            Assert.AreEqual(t.GetLeapSecond(), 18);
            Assert.AreEqual(t.ToGNSSTime().GetLeapSecond(), 18);
            t = new(1000, 1, 1);
            Assert.AreEqual(t.GetLeapSecond(), 0);
            Assert.AreEqual(t.ToGNSSTime().GetLeapSecond(), 0);
            t = new(1981, 7, 1);
            Assert.AreEqual(t.GetLeapSecond(), 1);
            Assert.AreEqual(t.ToGNSSTime().GetLeapSecond(), 1);
            t = new(2017, 1, 1);
            Assert.AreEqual(t.GetLeapSecond(), 18);
            Assert.AreEqual(t.ToGNSSTime().GetLeapSecond(), 18);
            t = t.AddSeconds(-1);
            Assert.AreEqual(t.GetLeapSecond(), 17);
            Assert.AreEqual(t.ToGNSSTime().GetLeapSecond(), 17);

        }
    }
}
