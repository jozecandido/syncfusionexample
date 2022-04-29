using FluentMigrator.Runner;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.ResponseCompression;
using QueryData;
using Services.Query;
using Services.Query.Hubs;


Console.Title = "Services.Query";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

var signalrbuilder = builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
})
.AddMessagePackProtocol(options =>
{
    options.SerializerOptions = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(TypelessObjectResolver.Instance, TypelessContractlessStandardResolver.Instance))
        .WithSecurity(MessagePackSecurity.UntrustedData);
});

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RemoteQueryExecutor>();


    builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(runnerBuilder => runnerBuilder
            .AddSqlServer2016()
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("Read"))
            .WithMigrationsIn(typeof(QueryDbDataContext).Assembly))
        .AddLogging(b => b.AddFluentMigratorConsole());


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder
        .WithOrigins("https://localhost:5100")
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseRouting();

app.MapHub<QueryHub>("/query");
app.MapControllers();


using var scope = app.Services.CreateScope();
var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();


app.Run();
