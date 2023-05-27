using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [Authorize(Roles = "ADMIN,VET,USER")]
    [ApiController]
    [Route("appointments")]
    public class AppointmentController: ControllerBase
    {
        private readonly AppointmentService Service;

        public AppointmentController(AppointmentService service)
        {
            Service = service;
        }

        [HttpGet]
        public ActionResult FindAllAppointments()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

        [HttpGet]
        public ActionResult FindAppointmentById(Guid id)
        {
            var task = Service.FindById(id);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        [HttpPost]
        public ActionResult CreateAppointment([FromBody] AppointmentDTOcreate appointment)
        {
            var err = appointment.Validate();
            if (err != null) return BadRequest(err);

            var task = Service.Create(appointment);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x =>
                {
                    if (x is AppointmentErrorBadRequest)
                    {
                        return BadRequest(x.Message);
                    }
                    else return NotFound(x.Message);
                }
                );
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAppointment(Guid id)
        {
            var task = Service.Delete(id);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x =>
                {
                    if (x is AppointmentErrorBadRequest)
                    {
                        return BadRequest(x.Message);
                    }
                    else return NotFound(x.Message);
                }
                );
        }
    }
}
