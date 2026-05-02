var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("data-base")
    .WithDataVolume();

var serverMain = builder.AddProject<Projects.Teledoc_ApiServices>("server-main")
    .WithReference(postgres)
    .WaitFor(postgres);


builder.Build().Run();

