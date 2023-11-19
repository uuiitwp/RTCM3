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

            Span<byte> a1 = stackalloc byte[2048];
            foreach (FileInfo file in GetFiles())
            {
                Console.WriteLine($"file name: {file.Name}");
                byte[] bs = File.ReadAllBytes(file.FullName);
                ReadOnlySequence<byte> rs1 = new(bs);
                ReadOnlySequence<byte> rs2 = new(bs);
                while (true)
                {
                    SequencePosition start = rs1.Start;
                    RTCM3.RTCM3? message = RTCM3.RTCM3.Filter(ref rs1);
                    SequencePosition end = rs1.Start;
                    ReadOnlySequence<byte> t = rs2.Slice(start, end);

                    rs2 = rs2.Slice(end);
                    if (rs1.IsEmpty || rs2.IsEmpty)
                    {
                        break;
                    }
                    if (message is null)
                    {
                        continue;
                    }
                    try
                    {
                        int? len = message.Databody?.Encode(ref a1);
                        byte[]? a = len is null ? null : a1[..len.Value].ToArray();
                        RTCM3_MSM46? c = message.Databody as RTCM3_MSM46;
                        byte[]? b = a is null ? null : t.Slice(t.Length - a.Length, a.Length).ToArray();
                        if (t.Length - a?.Length > 0)
                        {
                            string s = BitConverter.ToString(t.Slice(0, t.Length - a!.Length).ToArray()).Replace('-', ' ');
                            Console.WriteLine($"skip bytes: {s}");
                        }
                        bool? f = b is null ? null : a?.SequenceEqual(b);
                        if (f is null)
                        {
                            Console.WriteLine($"skip RTCM3 {message.MessageType}: Unsupported message type");
                        }
                        else if (f == true)
                        {
                            Console.WriteLine($"test RTCM3 {message.MessageType}");
                        }
                        if (f == false)
                        {
                            Console.WriteLine("test failed");
                        }
                        Assert.IsTrue(f ?? true);

                    }
                    catch (NotImplementedException)
                    {
                        Console.WriteLine($"skip RTCM3 {message.MessageType}: NotImplementedException");
                        continue;
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
                while (true)
                {
                    if (rs.IsEmpty)
                    {
                        break;
                    }
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

        [TestMethod]
        public void PassOrNot()
        {
            Assert.AreEqual(true, true);
        }
    }
}