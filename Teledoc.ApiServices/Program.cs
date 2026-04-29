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
        document.Info.Title = "Teldoc Api Services";
        document.Info.Version = "1.0.0";
        document.Info.Description = "Teldoc API Services test task";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Teldoc Api Services");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
