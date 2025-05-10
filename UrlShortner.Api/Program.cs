using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UrlShortner.Application;
using UrlShortner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/Logs-.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<UrlShortnerDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null
            )
        )
        .EnableSensitiveDataLogging()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        .EnableDetailedErrors()
    );

builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddSingleton<IUrlShortnerService, UrlShortnerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "My Awesome API",
        Description = "An ASP.NET Core Web API for managing awesome stuff",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "yourname@example.com",
            Url = new Uri("https://yourwebsite.com"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
        }
    });

    // Optional: Add XML comments (for method summaries in Swagger UI)
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Awesome API v1");
        options.RoutePrefix = string.Empty; // Swagger UI at root (localhost:5000/)
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapGet("/{shortCode}", async (string shortCode, [FromServices] ShortUrlRepository pShortUrlRepository) =>
{
    if (shortCode == "favicon.ico")
    {
        return Results.NotFound("Please give a worthy url");
    }

    var pJisShortUrl = await pShortUrlRepository.GetByShortCodeAsync(shortCode);

    if (pJisShortUrl == null)
    {
        return Results.NotFound("Short URL not found");
    }

    if (pJisShortUrl?.IsSuccess == true && pJisShortUrl.Data != null)
    {
        await pShortUrlRepository.IncrementClickCountAsync(pJisShortUrl.Data.JisUid);
        return Results.Redirect(pJisShortUrl.Data.JisOriginalUrl, permanent: true);
    }

    return Results.NotFound("Short URL not found");
})
.WithName("RedirectShortUrl")
.WithOpenApi(operation =>
{
    operation.Summary = "Redirects a short URL to its original destination";
    return operation;
});

app.Run();
