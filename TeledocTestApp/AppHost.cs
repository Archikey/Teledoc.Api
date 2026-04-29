var builder = DistributedApplication.CreateBuilder(args);

var serverMain = builder.AddProject<Projects.Teledoc_ApiServices>("server-main");


builder.Build().Run();
