var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache").WithRedisCommander();

//var webSite = builder.AddProject<Projects.Deliscio_Web_Site>("web-site")
//    .WithReference(cache);

var adminSite = builder.AddProject<Projects.Deliscio_Web_Admin>("admin-site")
    .WithReference(cache);

builder.Build().Run();
