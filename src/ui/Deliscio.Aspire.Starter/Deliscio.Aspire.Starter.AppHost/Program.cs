var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");

var apiservice = builder.AddProject<Projects.Deliscio.Aspire.Starter_ApiService>("apiservice");

builder.AddProject<Projects.Deliscio.Aspire.Starter_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.Build().Run();
