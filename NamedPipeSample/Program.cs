using System.Diagnostics;

var pipeServer = new Process();
var pipeServer2 = new Process();
var pipeClient = new Process();

pipeServer.StartInfo.FileName = "NamedPipeServer.exe";
pipeServer.StartInfo.Arguments = "pipe1";
pipeServer.StartInfo.UseShellExecute = true;
pipeServer.Start();

pipeServer2.StartInfo.FileName = "NamedPipeServer.exe";
pipeServer2.StartInfo.Arguments = "pipe1";
pipeServer2.StartInfo.UseShellExecute = true;
pipeServer2.Start();

pipeClient.StartInfo.FileName = "NamedPipeClient.exe";
pipeClient.StartInfo.Arguments = "pipe1";
pipeClient.StartInfo.UseShellExecute = true;
pipeClient.Start();