using Microsoft.EntityFrameworkCore;
using SportNewsAndScores.Api.Middleware;
using SportNewsAndScores.Core.Interfaces;
using SportNewsAndScores.Infrastructure.Data;
using SportNewsAndScores.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sport News and Scores API",
        Version = "v1",
        Description = "A comprehensive API for sports news, live scores, matches, and AI-powered commentary",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Sport News & Scores",
            Url = new Uri("https://github.com/gitfcankaya/sportnewsandscores")
        }
    });
});

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("SportNewsAndScores.Infrastructure")));

// Register services
builder.Services.AddHttpClient<INewsScraperService, NewsScraperService>();
builder.Services.AddHttpClient<IAICommentService, AICommentService>();
builder.Services.AddScoped<ILiveScoreService, LiveScoreService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Initialize database with migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Apply migrations
        context.Database.Migrate();
        
        // Seed data
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sport News and Scores API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Sport News & Scores API Documentation";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
