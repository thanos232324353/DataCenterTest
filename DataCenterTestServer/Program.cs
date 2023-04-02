using Dotmim.Sync;
using Dotmim.Sync.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30));


// [Required]: Get a connection string to your server data source
var connectionString = "server=192.0.0.192;database=AdventureWorks;uid=sa;pwd=ghfj;TrustServerCertificate=true";
// var connectionString = Configuration.GetSection("ConnectionStrings")["MySqlConnection"];

var options = new SyncOptions
{
    SnapshotsDirectory = "C:\\Tmp\\Snapshots",
    BatchSize = 2000,
};

// [Required] Tables involved in the sync process:
var tables = new string[] { "Product" };

// [Required]: Add a SqlSyncProvider acting as the server hub.
builder.Services.AddSyncServer<SqlSyncChangeTrackingProvider>(connectionString, tables, options);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
/*app.UseRouting()*/
;
app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();
