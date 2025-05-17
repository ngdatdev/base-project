using System.Text;
using Common.Middleware;
using Common.RegisterServices;
using Infrastructure.ServiceRegister;

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
