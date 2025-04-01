using AIResumeScoringAPI.Infrastructure.Services;
using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Database;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Load user secrets (for sensitive keys)
builder.Configuration.AddUserSecrets<Program>();

// ===================== Dependency Injection Setup =====================

// 🔹 Add Database Context (SQL Server)
builder.Services.AddDbContext<ResumeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Register Repository Interfaces & Implementations
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
builder.Services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
builder.Services.AddScoped<IResumeScoreRepository, ResumeScoreRepository>();

// 🔹 Register Scoring Service
builder.Services.AddScoped<IResumeScoringService, ResumeScoringService>();

// 🔹 Register Blob Storage Service (Singleton - can be reused globally)
builder.Services.AddSingleton<BlobStorageService>();

// 🔹 Register JWT Token Service (Singleton - stateless)
builder.Services.AddSingleton<JwtTokenService>();

// ===================== Swagger & API Docs =====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AI Resume Scoring API",
        Version = "v1",
        Description = "API for scoring candidate resumes against job descriptions using AI."
    });

    // 🔹 Enable file upload in Swagger
    options.OperationFilter<FileUploadOperationFilter>();

    // 🔹 Add JWT Bearer Authentication support in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}' to authorize"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// ===================== JWT Authentication Setup =====================
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = false, // Not validating audience in this case
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
    };
});

// ===================== Build & Run Application =====================
var app = builder.Build();

// 🔹 Enable Swagger UI in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Middleware pipeline
app.UseHttpsRedirection();

app.UseAuthentication(); // Must be before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
