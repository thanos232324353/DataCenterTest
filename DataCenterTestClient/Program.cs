using Dotmim.Sync;
using Dotmim.Sync.SqlServer;
using Dotmim.Sync.Web.Client;

Console.WriteLine("Be sure the web api has started. Then click enter..");
Console.ReadLine();
// Database script used for this sample : https://github.com/Mimetis/Dotmim.Sync/blob/master/CreateAdventureWorks.sql 

var serverOrchestrator = new WebRemoteOrchestrator("http://localhost:5252/api/Sync");

// Second provider is using plain old Sql Server provider, relying on triggers and tracking tables to create the sync environment
//var clientProvider = new SqlSyncProvider(clientConnectionString);
var clientProvider = new SqlSyncChangeTrackingProvider("server=192.0.0.192;database=Client;uid=sa;pwd=ghfj;TrustServerCertificate=true");


var options = new SyncOptions
{
    BatchSize = 1000
};

// Creating an agent that will handle all the process
var agent = new SyncAgent(clientProvider, serverOrchestrator, options);

do
{
    try
    {
        var progress = new SynchronousProgress<ProgressArgs>(args => Console.WriteLine($"{args.ProgressPercentage:p}:\t{args.Message}"));

        // Launch the sync process
        var s1 = await agent.SynchronizeAsync(progress);
        // Write results
        Console.WriteLine(s1);

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

} while (Console.ReadKey().Key != ConsoleKey.Escape);

Console.WriteLine("End");