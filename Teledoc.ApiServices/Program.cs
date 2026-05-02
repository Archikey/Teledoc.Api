using Teledoc.ApiServices.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Teledoc.ApiServices.Validators.Interfaces;
using Teledoc.ApiServices.Validators.Realisations;
using Teledoc.ApiServices.Repositorys.Interfaces;
using Teledoc.ApiServices.Repositorys.Realization;
using Teledoc.ApiServices.Services.Interfaces;
using Teledoc.ApiServices.Services.Realization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddLogging();

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Teledoc API Services";
        document.Info.Version = "1.0.0";
        document.Info.Description = "Teledoc API Services test task";
        return Task.CompletedTask;
    });
});


builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("data-base")));

builder.Services.AddSingleton<IInnValidator, InnValidator>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IManagerService, ManagerService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Teldoc Api Services");
        options.RoutePrefix = "swagger";
    });

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
