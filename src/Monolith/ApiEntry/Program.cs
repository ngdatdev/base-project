using System.Text;
using BaseApiReference.Entities;
using Common.Middleware;
using Common.RegisterServices;
using Infrastructure.Persistence.PostgreSQL.Data;
using Infrastructure.Persistence.PostgreSQL.DbContext;
using Infrastructure.ServiceRegister;
using Microsoft.AspNetCore.Identity;

// Default setting.
Console.OutputEncoding = Encoding.UTF8;
var entryAssembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Register services to the container.
services.RegisterCore(configuration, entryAssembly);
services.RegisterInfrastructure(configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seedingData = scope.ServiceProvider.GetRequiredService<DataSeeding>();

    // Can database be connected.
    var canConnect = await context.Database.CanConnectAsync();

    // Database cannot be connected.
    if (!canConnect)
    {
        throw new HostAbortedException(message: "Cannot connect database.");
    }

    // Try seed data.
    var seedResult = await seedingData.SeedAsync(
        context: context,
        userManager: scope.ServiceProvider.GetRequiredService<UserManager<User>>(),
        roleManager: scope.ServiceProvider.GetRequiredService<RoleManager<Role>>(),
        configuration: configuration,
        cancellationToken: CancellationToken.None
    );

    // Data cannot be seed.
    if (!seedResult)
    {
        throw new HostAbortedException(message: "Database seeding is false.");
    }
}

// Custom middleware
app.UseMiddleware<GlobalExceptionHandler>();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction: options =>
    {
        options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "v1");
        options.RoutePrefix = "swagger";
        options.DefaultModelsExpandDepth(depth: -1);
    });
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Enable response caching
app.UseResponseCaching();

// Rate limiting
app.UseRateLimiter();

// Authentication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// Map endpoints to controllers
app.MapControllers();

// Run app
await app.RunAsync(CancellationToken.None);
