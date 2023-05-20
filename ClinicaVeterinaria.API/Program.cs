using ClinicaVeterinaria.API.Api.controllers;
using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
                    Description = "An ASP.NET Core Web API for managing a veterinary clinic.",
                    /*
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                    */
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddDbContext<ClinicaDBContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // DI de los repositorios
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<VetRepository>();
            builder.Services.AddScoped<AppointmentRepository>();
            builder.Services.AddScoped<HistoryRepository>();
            builder.Services.AddScoped<VaccineRepository>();
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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}