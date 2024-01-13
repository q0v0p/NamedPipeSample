using System.Diagnostics;

var pipeServer = new Process();
var pipeClient = new Process();

pipeServer.StartInfo.FileName = "NamedPipeServer.exe";
pipeServer.StartInfo.Arguments = "pipe1";
pipeServer.StartInfo.UseShellExecute = true;
pipeServer.Start();

pipeClient.StartInfo.FileName = "NamedPipeClient.exe";
pipeClient.StartInfo.Arguments = "pipe1";
pipeClient.StartInfo.UseShellExecute = true;
pipeClient.Start();