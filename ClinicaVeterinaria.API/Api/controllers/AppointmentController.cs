using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.API.Api.controllers
{
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
        [HttpGet, Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindAllAppointments(string? userEmail, string? vetEmail, string? date)
        {
            var successful = DateOnly.TryParse(date, out DateOnly dateOnly);
            Task<List<AppointmentDTOshort>> task;
            if (successful) { task = Service.FindAll(userEmail, vetEmail, dateOnly); }
            else if (!successful && date != null) { return BadRequest("Date is not in a valid format."); }
            else task = Service.FindAll(userEmail, vetEmail, null);
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
        [HttpGet("{id}"), Authorize(Roles = "ADMIN,VET,USER")]
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
        [HttpPost, Authorize(Roles = "ADMIN,VET,USER")]
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

        /// <summary>
        /// Updates an appointment's state, if an appointment with that ID exists.
        /// </summary>
        /// <returns>
        /// The appointment, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
        [HttpPut("{id}"), Authorize(Roles = "ADMIN,VET")]
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
        [HttpDelete("{id}"), Authorize(Roles = "ADMIN,VET,USER")]
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
