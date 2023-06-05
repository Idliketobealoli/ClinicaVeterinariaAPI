using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("history")]
    public class HistoryController: ControllerBase
    {
        private readonly HistoryService Service;

        public HistoryController(HistoryService service)
        {
            Service = service;
        }

        /// <summary>
        /// Finds all histories present in the database.
        /// </summary>
        /// <returns>
        /// A list containing all histories.
        /// </returns>
        /// <response code="200" />
        [HttpGet, Authorize(Roles = "ADMIN,VET")]
        public ActionResult FindAllHistories()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

        /// <summary>
        /// Finds a history by its pet ID.
        /// </summary>
        /// <returns>
        /// The history, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindHistoryByPetId(Guid id)
        {
            var task = Service.FindByPetId(id);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Finds a history by its pet ID, but only returns the vaccination history.
        /// </summary>
        /// <returns>
        /// The history, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("vaccinesonly/{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindHistoryByPetIdVaccinesOnly(Guid id)
        {
            var task = Service.FindByPetIdVaccinesOnly(id);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Finds a history by its pet ID, but only returns the ailment and treament history.
        /// </summary>
        /// <returns>
        /// The history, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("ailmentonly/{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindHistoryByPetIdAilmTreatOnly(Guid id)
        {
            var task = Service.FindByPetIdAilmTreatOnly(id);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Adds a vaccine to the history that corresponds with the given petId.
        /// </summary>
        /// <returns>
        /// The history, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
        [HttpPut("vaccine/{id}"), Authorize(Roles = "ADMIN,VET")]
        public ActionResult AddVaccine(Guid id, [FromBody] VaccineDTO vaccine)
        {
            var err = vaccine.Validate();
            if (err != null)
                return BadRequest(err);

            var task = Service.AddVaccine(id, vaccine);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Adds an ailment-treatment pair to the history that corresponds with the given petId.
        /// </summary>
        /// <returns>
        /// The history, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
        [HttpPut("ailment/{id}"), Authorize(Roles = "ADMIN,VET")]
        public ActionResult AddAilmentTreatment(Guid id, [FromBody] AilmentTreatmentDTO ailmentTreatment)
        {
            var err = ailmentTreatment.Validate();
            if (err != null)
                return BadRequest(err);

            var task = Service.AddAilmentTreatment(id, ailmentTreatment);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }
    }
}
