# RTCM3 Encoder/Decoder

This project is an RTCM3 encoder/decoder written in C#.

## Releases

https://www.nuget.org/packages/RTCM3

## How to Use

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
    
    if (message != null)
    {
        try
        {
            if (message.Databody != null)
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

## Contributing

Contributions of all kinds are welcome! If you find an error or have a suggestion for a new feature, feel free to submit an issue or pull request.

## License

This project is licensed under the MIT License.
