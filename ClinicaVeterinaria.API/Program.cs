using ClinicaVeterinaria.API.Api.controllers;
using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services;
using Microsoft.EntityFrameworkCore;
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

            string? connectionString = builder.Configuration.GetConnectionString("default_connection");
            builder.Services.AddPooledDbContextFactory<ClinicaDBContext>(o => o.UseNpgsql(connectionString));

            builder.Services.AddSingleton<UserRepository>();
            builder.Services.AddSingleton<VetRepository>();
            builder.Services.AddSingleton<AppointmentRepository>();
            builder.Services.AddSingleton<HistoryRepository>();
            builder.Services.AddSingleton<VaccineRepository>();
            builder.Services.AddSingleton<PetRepository>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<VetService>();
            builder.Services.AddSingleton<AppointmentService>();
            builder.Services.AddSingleton<HistoryService>();
            builder.Services.AddSingleton<PetService>();
            builder.Services.AddSingleton<UserController>();
            builder.Services.AddSingleton<VetController>();
            builder.Services.AddSingleton<AppointmentController>();
            builder.Services.AddSingleton<HistoryController>();
            builder.Services.AddSingleton<PetController>();
            builder.Services.AddDbContext<ClinicaDBContext>(); // POSIBLE CAMBIAR

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