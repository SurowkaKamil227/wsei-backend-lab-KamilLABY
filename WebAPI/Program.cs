
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models;
using Infrastructure;
using Infrastructure.EF.Entities;
using Infrastructure.Memory.Repository;
using Infrastructure.MongoDB.Entities;
using Infrastructure.MongoDB;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using WseiBackendLab.Configuration;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<QuizDbContext>();
            // Add services to the container.
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
            builder.Services.AddSingleton<QuizUserServiceMongoDB>();
            builder.Services.AddSingleton<JwtSettings>();
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJWT(new JwtSettings(builder.Configuration));
            builder.Services.ConfigureCors();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
              Enter 'Bearer' and then your token in the text input below.
              Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Quiz API",
                });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
           

                app.MapControllers();
            app.AddUsers();
            app.Run();
        }
    }
    
}
public partial class Program
{
}