using ClinicaVeterinaria.API.Api.controllers;
using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace ClinicaVeterinaria.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Clinica Veterinaria API",
                    Description = "An ASP.NET Core Web API for managing a veterinary clinic."
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
                    );
            });

            builder.Services.AddDbContext<ClinicaDBContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // DI de los repositorios
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<VetRepository>();
            builder.Services.AddScoped<AppointmentRepository>();
            builder.Services.AddScoped<HistoryRepository>();
            builder.Services.AddScoped<VaccineRepository>();
            builder.Services.AddScoped<AilmentTreatmentRepository>();
            builder.Services.AddScoped<PetRepository>();

            // DI de los servicios
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<VetService>();
            builder.Services.AddScoped<AppointmentService>();
            builder.Services.AddScoped<HistoryService>();
            builder.Services.AddScoped<PetService>();

            // DI de los controladores
            builder.Services.AddScoped<UserController>();
            builder.Services.AddScoped<VetController>();
            builder.Services.AddScoped<AppointmentController>();
            builder.Services.AddScoped<HistoryController>();
            builder.Services.AddScoped<PetController>();

            var sqlOptionsBuilder = new DbContextOptionsBuilder<ClinicaDBContext>()
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            var vetsRepo = new VetRepository(new ClinicaDBContext(sqlOptionsBuilder.Options));

            InitialData.SetData(vetsRepo);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            //app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
