var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.Run();
