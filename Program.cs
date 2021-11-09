// See https://aka.ms/new-console-template for more information
using PinTcpRedirect.Client;
using PinTcpRedirect.Server;

Console.WriteLine(String.Join(" ", args));
List<Task> allTasks = new();
if (args.Contains("--server"))
{
    Console.WriteLine("Running As Server");
    var server = new ServerTask(2221, 2201);
    allTasks.Add(Task.Run(() => server.PinToClient()));
    allTasks.Add(Task.Run(() => server.FromClient()));
}
else
{
    Console.WriteLine("Running As Client");
    var client = new ClientTask("127.0.0.1", 2221, "40.85.228.169", 2201, 4000);
    allTasks.Add(Task.Run(() => client.ServerToClientSideTask()));
    allTasks.Add(Task.Run(() => client.LocalRiToServerTask()));
}

Task.WaitAll(allTasks.ToArray());