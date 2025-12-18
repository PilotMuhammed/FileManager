using fileManager.Api.Data;
using fileManager.Api.IRepositories;
using fileManager.Api.Mapster;
using fileManager.Api.Middleware;
using fileManager.Api.Repositories;
using fileManager.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScopedServices();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


MappingConfig.ConfigureMappings();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();


builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();



app.UseRouting();
app.UseCors("Cors");
app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
