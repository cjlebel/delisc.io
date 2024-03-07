var builder = DistributedApplication.CreateBuilder(args);

var cacheAdmin = builder.AddRedis("cache-admin");
var cacheWeb = builder.AddRedis("cache-web");

builder.AddProject<Projects.Deliscio_Admin>("admin-site")
    .WithReference(cacheAdmin);

builder.AddProject<Projects.Deliscio_Web_Mvc>("web-site")
    .WithReference(cacheWeb);

builder.Build().Run();
