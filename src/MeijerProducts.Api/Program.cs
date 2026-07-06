using MeijerProducts.Api.Data;
using MeijerProducts.Api.Repositories;
using MeijerProducts.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Services
// ---------------------------------------------------------------------------
builder.Services.AddControllers();

// Swagger / OpenAPI for interactive docs and easy manual testing.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistence: EF Core over SQLite. Connection string is configurable via appsettings.
var connectionString = builder.Configuration.GetConnectionString("ProductDatabase")
    ?? "Data Source=products.db";
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlite(connectionString));

// Application layers.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Allow the MAUI client (and Swagger from other origins) to call the API during development.
const string CorsPolicy = "AllowClients";
builder.Services.AddCors(options =>
    options.AddPolicy(CorsPolicy, policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Consistent RFC 7807 ProblemDetails responses for unhandled errors and status codes.
builder.Services.AddProblemDetails();

var app = builder.Build();

// ---------------------------------------------------------------------------
// Seed the database on startup.
// ---------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ProductDbContext>();
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
    await DbSeeder.SeedAsync(context, app.Environment, logger);
}

// ---------------------------------------------------------------------------
// HTTP pipeline
// ---------------------------------------------------------------------------
app.UseExceptionHandler();        // Turns unhandled exceptions into ProblemDetails.
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);
app.MapControllers();

app.Run();

// Exposed so the integration/unit test project can reference the entry-point assembly
// via WebApplicationFactory<Program> if desired.
public partial class Program { }
