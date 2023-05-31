using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Finds all appointments in the database.
        /// </summary>
        /// <returns>
        /// A list of all appointments.
        /// </returns>
        /// <response code="200" />
        [HttpGet]
        public ActionResult FindAllAppointments(string? userEmail, string? vetEmail, string? date)
        {
            _ = DateOnly.TryParse(date, out DateOnly dateOnly);
            var task = Service.FindAll(userEmail, vetEmail, dateOnly);
            task.Wait();

            return Ok(task.Result);
        }

        /// <summary>
        /// Finds an appointment by its ID.
        /// </summary>
        /// <returns>
        /// The appointment, if found.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("{id}")]
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

        /// <summary>
        /// Creates an appointment, if the body of the request is valid and it can be created.
        /// </summary>
        /// <returns>
        /// The appointment, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
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

        [HttpPatch("{id}")]
        public ActionResult UpdateAppointment(Guid id, [Required] string state)
        {
            var task = Service.UpdateState(id, state);
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

        /// <summary>
        /// Deletes an appointment by ID.
        /// </summary>
        /// <returns>
        /// The appointment, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
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
