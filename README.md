# RTCM3 Encoder/Decoder

This project is an RTCM3 encoder/decoder written in C#.

## Releases

https://www.nuget.org/packages/RTCM3

## How to Use

```C#
RTCM3.RTCM3? message = RTCM3.RTCM3.Filter(ref buffer);
```

- ```Filter``` is always used in conjunction with ```PipeReader```
- ```pipeReader.AdvanceTo(buffer.Start, buffer.End)``` should be called after ```Filter```

```C#
Span<byte> span = stackalloc byte[2048];
FileInfo file = new FileInfo("test.rtcm3");
Console.WriteLine($"file name: {file.Name}");
using var fs = File.OpenRead(file.FullName);
PipeReader pipeReader = PipeReader.Create(fs);
while (true)
{
    var rs = pipeReader.ReadAsync().AsTask().Result;
    var buffer = rs.Buffer;
    RTCM3.RTCM3? message = RTCM3.RTCM3.Filter(ref buffer);
    
    // decoded RTCM3 data
    
    if (message is not null)
    {
        try
        {
            if (message.Databody is not null)
            {
                var len = message.Databody.Encode(ref span);
                // encoded RTCM3 data
            }
        }
        catch (NotImplementedException)
        {
            Console.WriteLine($"RTCM_{message.MessageType} Encode method is not implemented.");
        }
    }
    pipeReader.AdvanceTo(buffer.Start, buffer.End); // important
    if (rs.IsCompleted)
    {
        break;
    }
}
```

- ```SyncMSMsFilter``` is used to filter MSM by epoch

```C#
FileInfo file = new FileInfo("test.rtcm3");
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
        foreach (RTCM3.RTCM3 msm in msms)
        {
            RTCM3_MSM? m = msm.Databody as RTCM3_MSM;
            Observation[]? o = m?.GetObservations();
            if (o is not null && m is not null)
            {
                foreach (var obs in o)
                {
                    Console.WriteLine($"StationID={m.StationID} GNSSTime={obs.GNSSTime} {obs} CarrierPhase={obs.carrierPhase} PseudoRange={obs.pseudoRange}");
                }
            }
        }
    }
    pipeReader.AdvanceTo(buffer.Start, buffer.End);
    if (rs.IsCompleted)
    {
        break;
    }
}

// output:
// file name: test.rtcm3

// StationID=1823 GNSSTime=2024/10/26 04:25:04.0000000 +NanoSecond=0 LeapSecond=18 C20 2I CNR=44 CarrierPhase=123585908.83753031 PseudoRange=23733329.741727743
// StationID=1823 GNSSTime=2024/10/26 04:25:04.0000000 +NanoSecond=0 LeapSecond=18 C20 6I CNR=44 CarrierPhase=100423674.26112795 PseudoRange=23733317.197673623
// StationID=1823 GNSSTime=2024/10/26 04:25:04.0000000 +NanoSecond=0 LeapSecond=18 C20 5X CNR=46 CarrierPhase=93134859.14777051 PseudoRange=23733327.79400424
// StationID=1823 GNSSTime=2024/10/26 04:25:04.0000000 +NanoSecond=0 LeapSecond=18 C20 7D CNR=44 CarrierPhase=95564463.71257658 PseudoRange=23733325.524638325
// StationID=1823 GNSSTime=2024/10/26 04:25:04.0000000 +NanoSecond=0 LeapSecond=18 C20 1X CNR=43 CarrierPhase=124719724.65872683 PseudoRange=23733324.452496946
// StationID=1823 GNSSTime=2024/10/26 04:25:04.0000000 +NanoSecond=0 LeapSecond=18 C27 2I CNR=37 CarrierPhase=134467859.46314394 PseudoRange=25823044.486235183
// StationID=1823 GNSSTime=2024/10/26 04:25:04.0000000 +NanoSecond=0 LeapSecond=18 C27 6I CNR=37 CarrierPhase=109266150.36242388 PseudoRange=25823040.483574037
// ...
```

## Contributing

Contributions of all kinds are welcome! If you find an error or have a suggestion for a new feature, feel free to submit an issue or pull request.

## License

This project is licensed under the MIT License.
