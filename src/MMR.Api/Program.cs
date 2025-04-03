using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateSlimBuilder(args);

builder.WebHost.UseKestrel(kestrel => kestrel.AddServerHeader = false);

builder.Services.Configure<JsonSerializerOptions>(jsonOptions =>
{
    jsonOptions.IncludeFields = true;
    jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
});

builder.Services.AddOpenApi();
builder.Services.AddLocalization();
builder.Services.AddRequestLocalization(localization =>
{
    localization.SupportedUICultures = [
        new CultureInfo("en"),
        new CultureInfo("ru")
    ];

    localization.SetDefaultCulture("en");
});

builder.Services
    .AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
    {
        jwtOptions.MapInboundClaims = false;
        jwtOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
    });

builder.Services
    .AddAuthorizationBuilder()
    .AddDefaultPolicy(
        "User",
        policy => policy.RequireClaim("user_id ").RequireClaim("email")
    );

var app = builder.Build();

app.MapOpenApi();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();
