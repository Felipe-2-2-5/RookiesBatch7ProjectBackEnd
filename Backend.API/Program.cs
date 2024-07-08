using Backend.Application.AuthProvide;
using Backend.Application.Common.Converter;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Application.DTOs.CategoryDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Middleware;
using Backend.Application.Services.AssetServices;
using Backend.Application.Services.AssignmentServices;
using Backend.Application.Services.CategoryServices;
using Backend.Application.Services.ReportServices;
using Backend.Application.Services.ReturnRequestServices;
using Backend.Application.Services.UserServices;
using Backend.Application.Validations;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Db connection
var connectionString = builder.Configuration.GetConnectionString("AssetManager");
builder.Services.AddDbContext<AssetContext>(options => options.UseSqlServer(connectionString));

//Add Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Add JWT
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = TokenService.GetTokenValidationParameters(builder.Configuration);
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/api/userStateHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });


//Add Swagger authen
builder.Services.AddSwaggerGen(opt =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    opt.AddSecurityDefinition("Bearer", securitySchema);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    });
});

//Add the CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.SetIsOriginAllowed(origin => true)
               .AllowCredentials()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Authorization", "Content-Disposition");
    });
});
// Token services
builder.Services.AddScoped<ITokenService, TokenService>();

//User services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();

//Category services
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

//Asset services
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IReportService, ReportService>();

//Return request services
builder.Services.AddScoped<IReturnRequestRepository, ReturnRequestRepository>();
builder.Services.AddScoped<IReturnRequestService, ReturnRequestService>();

//Add FluentValidation services
builder.Services.AddTransient<IValidator<UserDTO>, UserValidator>();
builder.Services.AddTransient<IValidator<CategoryDTO>, CategoryValidator>();
builder.Services.AddTransient<IValidator<AssetDTO>, AssetValidator>();
builder.Services.AddTransient<IValidator<AssignmentDTO>, AssignmentValidator>();


//Add SignalR
builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();