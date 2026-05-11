using CrudApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// SQL Server Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// CORS for React App
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:3000",
                "https://creact-api.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger always enabled
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Root test endpoint
app.MapGet("/", () => "API is running online");

// Health test endpoint
app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "API is running",
        message = "ASP.NET Core API hosted successfully"
    });
});

app.Run();