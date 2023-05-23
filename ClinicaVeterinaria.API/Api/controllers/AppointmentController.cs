using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [Authorize(Roles = "ADMIN,VET,USER")]
    [ApiController]
    [Route("appointments")]
    public class AppointmentController
    {
        private readonly AppointmentService Service;

        public AppointmentController(AppointmentService service)
        {
            Service = service;
        }

        [HttpGet]
        public IResult FindAllAppointments()
        {
            var task = Service.FindAll();
            task.Wait();

            return Results.Ok(task.Result);
        }

        [HttpGet]
        public IResult FindAppointmentById(Guid id)
        {
            var task = Service.FindById(id);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpPost]
        public IResult CreateAppointment([FromBody] Appointment appointment)
        {
            var task = Service.Create(appointment);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x =>
                {
                    if (x is AppointmentErrorBadRequest)
                    {
                        return Results.BadRequest(x.Message);
                    }
                    else return Results.NotFound(x.Message);
                }
                );
        }

        [HttpDelete("{id}")]
        public IResult DeleteAppointment(Guid id)
        {
            var task = Service.Delete(id);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x =>
                {
                    if (x is AppointmentErrorBadRequest)
                    {
                        return Results.BadRequest(x.Message);
                    }
                    else return Results.NotFound(x.Message);
                }
                );
        }
    }
}
