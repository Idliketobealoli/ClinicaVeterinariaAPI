using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("history")]
    public class HistoryController
    {
        private readonly HistoryService Service;

        public HistoryController(HistoryService service)
        {
            Service = service;
        }

        [HttpGet, Authorize(Roles = "ADMIN,VET")]
        public IResult FindAllHistories()
        {
            var task = Service.FindAll();
            task.Wait();

            return Results.Ok(task.Result);
        }

        [HttpGet("{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public IResult FindHistoryByPetId(Guid id)
        {
            var task = Service.FindByPetId(id);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpGet("vaccinesonly/{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public IResult FindHistoryByPetIdVaccinesOnly(Guid id)
        {
            var task = Service.FindByPetIdVaccinesOnly(id);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpGet("ailmentonly/{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public IResult FindHistoryByPetIdAilmTreatOnly(Guid id)
        {
            var task = Service.FindByPetIdAilmTreatOnly(id);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpPut("vaccine/{id}"), Authorize(Roles = "ADMIN,VET")]
        public IResult AddVaccine(Guid id, [FromBody] VaccineDTO vaccine)
        {
            var err = vaccine.Validate();
            if (err != null)
                return Results.BadRequest(err);

            var task = Service.AddVaccine(id, vaccine);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpPost("ailment/{id}"), Authorize(Roles = "ADMIN,VET")]
        public IResult AddAilmentTreatment(Guid id, [FromHeader] string ailment, [FromBody] string treatment)
        {
            var task = Service.AddAilmentTreatment(id, ailment, treatment);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }
    }
}
