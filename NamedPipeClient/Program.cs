using System.IO.Pipes;
using System.Security.Principal;
using System.Text;

if (args.Length <= 0)
    return;

var pipeName = args[0];

Console.WriteLine("Start NamedPipeClient.");
Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} Please enter a message, and then press Enter.");

// Display the read text to the console
string write = string.Empty;

do
{
    try
    {
        using var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
        pipeClient.Connect();

        var ss = new StreamString(pipeClient);

        while (true)
        {
            write = Console.ReadLine();
            var writeData = ss.WriteString(write);
            var read = ss.ReadString();

            Console.WriteLine($"Server Response = {read}");

            if (write == "end") break;
        }
    }
    catch (OverflowException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (IOException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
while (!write.StartsWith("end"));

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