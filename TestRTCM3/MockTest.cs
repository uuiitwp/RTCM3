using System.IO.Pipelines;

namespace TestRTCM3
{
    [TestClass]
    public class MockTest
    {
        private static FileInfo[] GetFiles()
        {
            DirectoryInfo directoryInfo = new("RTCM3_bin");
            return directoryInfo.GetFiles("*.rtcm3", SearchOption.AllDirectories);
        }

        [TestMethod, Timeout(100000)]
        public void DecodeEncodeRTCM3_01()
        {
            DecodeEncodeRTCM3(out var MessageCount);
            Console.WriteLine($"Total Message Count is {MessageCount}");
        }

        [TestMethod, Timeout(100000)]
        public void DecodeEncodeRTCM3_02()
        {
            // pipeReader.ReadAsync does not read a complete package
            StreamPipeReaderOptions streamPipeReaderOptions = new(bufferSize: 1);
            DecodeEncodeRTCM3(out var MessageCount, streamPipeReaderOptions);
            Console.WriteLine($"Total Message Count is {MessageCount}");
            DecodeEncodeRTCM3(out var MessageCount1);
            Console.WriteLine($"Total Message Count is {MessageCount1}");
            Assert.AreEqual(MessageCount, MessageCount1);
        }


        private void DecodeEncodeRTCM3(out long MessageCount, StreamPipeReaderOptions? streamPipeReaderOptions = null)
        {
            MessageCount = 0L;
            Span<byte> span = stackalloc byte[2048];
            foreach (FileInfo file in GetFiles())
            {
                Console.WriteLine($"file name: {file.Name}");
                using FileStream fs = File.OpenRead(file.FullName);
                PipeReader pipeReader = PipeReader.Create(fs, streamPipeReaderOptions);
                while (true)
                {
                    ReadResult rs = pipeReader.ReadAsync().AsTask().Result;
                    ReadOnlySequence<byte> buffer = rs.Buffer;
                    ReadOnlySequence<byte> read = buffer.Slice(0);
                    RTCM3.RTCM3? message = RTCM3.RTCM3.Filter(ref buffer);
                    if (message != null)
                    {
                        Console.WriteLine($"RTCM3_{message.MessageType} decoded");
                        MessageCount++;
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
                        Assert.IsTrue(MessageCount > 0, "MessageCount is zeor");
                        break;
                    }
                }
            }

        }


        [TestMethod, Timeout(100000)]
        public void DecodeSyncMSM()
        {
            foreach (FileInfo file in GetFiles())
            {
                Console.WriteLine($"file name: {file.Name}");
                SyncMSMsFilter Filter = new();
                using FileStream fs = File.OpenRead(file.FullName);
                PipeReader pipeReader = PipeReader.Create(fs);
                while (true)
                {
                    ReadResult rs = pipeReader.ReadAsync().AsTask().Result;
                    ReadOnlySequence<byte> buffer = rs.Buffer;
                    RTCM3.RTCM3[]? msms = Filter.Filter(ref buffer);
                    if (msms is not null)
                    {
                        Console.WriteLine();
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
                                if (o is not null && m is not null)
                                {
                                    foreach (var obs in o)
                                    {
                                        Console.WriteLine($"StationID={m.StationID} GNSSTime={obs.GNSSTime} {obs} CarrierPhase={obs.carrierPhase} PseudoRange={obs.pseudoRange}");
                                    }
                                }
                                IEnumerable<GNSSTime>? t = o?.Select(x => x.GNSSTime);
                                if (t is not null)
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
                    pipeReader.AdvanceTo(buffer.Start, buffer.End);
                    if (rs.IsCompleted)
                    {
                        break;
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
