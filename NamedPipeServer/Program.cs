using System.IO.Pipes;
using System.Text;

if (args.Length <= 0)
    return;

var pipeName = args[0];
var streamSize = 1;

Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} Please enter a message, and then press Enter.");

// Display the read text to the console
string read = string.Empty;

do
{
    try
    {
        using var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, streamSize);
        pipeServer.WaitForConnection();

        var ss = new StreamString(pipeServer);

        while (true)
        {
            read = ss.ReadString();
            var write = ss.WriteString("Server read OK.");

            Console.WriteLine($"Read Data = {read}");

            if (read == "end") break;
        }
    }
    catch (OverflowException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
while (!read.StartsWith("end"));

public class StreamString
{
    private Stream ioStream;
    private UnicodeEncoding streamEncoding;

    public StreamString(Stream ioStream)
    {
        this.ioStream = ioStream;
        streamEncoding = new UnicodeEncoding();
    }

    public string ReadString()
    {
        int len = 0;

        len = ioStream.ReadByte() * 256;
        len += ioStream.ReadByte();
        byte[] inBuffer = new byte[len];
        ioStream.Read(inBuffer, 0, len);

        return streamEncoding.GetString(inBuffer);
    }

    public int WriteString(string outString)
    {
        byte[] outBuffer = streamEncoding.GetBytes(outString);
        int len = outBuffer.Length;
        if (len > UInt16.MaxValue)
        {
            len = (int)UInt16.MaxValue;
        }
        ioStream.WriteByte((byte)(len / 256));
        ioStream.WriteByte((byte)(len & 255));
        ioStream.Write(outBuffer, 0, len);
        ioStream.Flush();

        return outBuffer.Length + 2;
    }
}
