// See https://aka.ms/new-console-template for more information
using PinTcpRedirect.Client;
using PinTcpRedirect.Server;

Console.WriteLine("Running As Server");
Console.WriteLine("Running As Client");
Console.WriteLine(String.Join(" ", args));
List<Task> allTasks = new();
if (args.Contains("--server"))
{
    var server = new ServerTask(2221, 2201);
    allTasks.Add(Task.Run(() => server.PinToClient()));
    allTasks.Add(Task.Run(() => server.FromClient()));
}
else
{
    var client = new ClientTask("127.0.0.1", 2201, "40.85.228.169", 2202, 4000);
    allTasks.Add(Task.Run(() => client.ServerToClientSideTask()));
    allTasks.Add(Task.Run(() => client.LocalRiToServerTask()));
}

Task.WaitAll(allTasks.ToArray());