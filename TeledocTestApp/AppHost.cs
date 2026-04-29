var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis-cash");

var postgres = builder.AddPostgres("data-base")
    .WithDataVolume();

var serverMain = builder.AddProject<Projects.Teledoc_ApiServices>("server-main")
    .WithReference(redis)
    .WithReference(postgres)
    .WaitFor(redis)
    .WaitFor(postgres);


builder.Build().Run();

