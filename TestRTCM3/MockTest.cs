using RTCM3;
using RTCM3.Common.Time;
using RTCM3.RTCM3Message;
using System.Buffers;



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
                    byte[] b = rs2.Slice(start, end).ToArray();
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
                        byte[]? a = message.Databody?.Encode().ToArray();
                        RTCM3_MSM46? c = message.Databody as RTCM3_MSM46;
                        c?.GetObservations();
                        bool? f = a?.SequenceEqual(b);
                        if (f is null)
                        {
                            Console.WriteLine($"skip RTCM3 {message.MessageType}");
                        }
                        else if (f == true)
                        {
                            Console.WriteLine($"test RTCM3 {message.MessageType}");
                        }
                        Assert.IsTrue(f ?? true);

                    }
                    catch (NotImplementedException)
                    {
                        Console.WriteLine($"skip RTCM3 {message.MessageType}");
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
                SyncMSMFilter Filter = new SyncMSMFilter();
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
                    }
                }
            }
        }


        [TestMethod]
        public void TestLeepsecond()
        {
            DateTime t = new(2022, 1, 1);
            Assert.AreEqual(t.GetLeapSecond(), 18);
            t = new DateTime(1000, 1, 1);
            Assert.AreEqual(t.GetLeapSecond(), 0);
            Assert.AreEqual(LeapSecond.GetLeapSecond(new(1981, 7, 1)), 1);
            Assert.AreEqual(LeapSecond.GetLeapSecond(new(2017, 1, 1)), 18);
        }

        [TestMethod]
        public void PassOrNot()
        {
            Assert.AreEqual(true, true);
        }
    }
}