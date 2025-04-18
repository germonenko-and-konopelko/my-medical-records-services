using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MMR.Api.FluentValidation;
using MMR.Api.Middleware;
using MMR.Common.Api;
using MMR.Common.Api.Versioning;
using MMR.Common.Data;
using MMR.Common.Encoding;
using MMR.Patient;
using Sqids;

ValidatorOptions.Global.LanguageManager = new MmrLanguageManager();

var builder = WebApplication.CreateSlimBuilder(args);

builder.WebHost.UseKestrel(kestrel => kestrel.AddServerHeader = false);

var sqidsEncoder = new SqidsEncoder<long>(new SqidsOptions
{
    Alphabet = builder.Configuration.GetRequiredValue<string>("Sqids:Alphabet"),
});
var encoder = new Encoder(sqidsEncoder);
builder.Services.AddSingleton<IEncoder>(encoder);

builder.Services.ConfigureHttpJsonOptions(jsonOptions =>
{
    var serializerOptions = jsonOptions.SerializerOptions;
    serializerOptions.IncludeFields = true;
    serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
    serializerOptions.Converters.Add(new PossiblyUndefinedJsonConverterFactory());
    serializerOptions.Converters.Add(new EncodedLongJsonConverter(encoder));
});

builder.Services.AddDbContext<MmrDatabaseContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres");
    options.UseNpgsql(
        connectionString,
        postgresBuilder => {
            postgresBuilder.MigrationsAssembly("MMR.Common.Data");
            postgresBuilder.MigrationsHistoryTable("migrations_history", "system");
        });
    options.UseSnakeCaseNamingConvention();
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

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
    .AddDefaultPolicy("DefinedUser", policy => policy.RequireClaim("user_id"));

builder.Services.AddScoped<UserContext>();
builder.Services.AddPatientModule();

builder.Services.AddSingleton<SetUserContextMiddleware>();
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<SetUserContextMiddleware>();

var v1Preview = app
    .MapGroup($"api/{Versions.V1Preview}")
    .RequireAuthorization();

v1Preview.MapPatientEndpoints();

app.Run();
