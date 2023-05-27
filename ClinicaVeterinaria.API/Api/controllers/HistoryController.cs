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

        [HttpGet, Authorize(Roles = "ADMIN,VET")]
        public ActionResult FindAllHistories()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

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

        [HttpPost("ailment/{id}"), Authorize(Roles = "ADMIN,VET")]
        public ActionResult AddAilmentTreatment(Guid id, [FromHeader] string ailment, [FromBody] string treatment)
        {
            var task = Service.AddAilmentTreatment(id, ailment, treatment);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }
    }
}
