using MCComputersBackend.Data;
using MCComputersBackend.Repository.Interface;
using MCComputersBackend.Repository.Implementation;
using MCComputersBackend.Service.Interface;
using MCComputersBackend.Service.Implementation;
using MCComputersBackend.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage);
            
            var result = new
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = errors
            };
            
            return new BadRequestObjectResult(result);
        };
    });

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MCComputersDB"));

// Register Repository dependencies
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Register Service dependencies
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "MCComputers API", 
        Version = "v1",
        Description = "API for MCComputers Backend System - Manages products and invoices for computer retail business",
        Contact = new OpenApiContact 
        { 
            Name = "MCComputers Team", 
            Email = "support@mccomputers.com" 
        }
    });
    
    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MCComputers API v1");
    c.RoutePrefix = string.Empty; // Set Swagger as the default page
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();
    c.EnableValidator();
});

// Add custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Add a simple default route
app.MapGet("/", () => "MCComputers Backend API is running! Visit /swagger for API documentation.").ExcludeFromDescription();

app.MapControllers();

app.Run();
