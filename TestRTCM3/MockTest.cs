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
        public void Mock()
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
                    if (message is null)
                    {
                        if (start.Equals(rs1.End))
                        {
                            break;
                        }
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
        public void Fail()
        {
            Assert.AreEqual(true, false);
        }
    }
}