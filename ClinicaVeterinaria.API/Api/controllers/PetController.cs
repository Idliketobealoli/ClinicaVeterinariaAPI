using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("pets")]
    public class PetController: ControllerBase
    {
        private readonly PetService Service;

        public PetController(PetService service)
        {
            Service = service;
        }

        [HttpGet, Authorize(Roles = "ADMIN,VET")]
        public ActionResult FindAllPets()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

        [HttpGet("{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindPetById(Guid id)
        {
            var task = Service.FindById(id);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        [HttpPost, Authorize(Roles = "ADMIN,VET")]
        public ActionResult CreatePet([FromBody] PetDTOcreate dto)
        {
            var err = dto.Validate();
            if (err != null) return BadRequest(err);

            var task = Service.Create(dto);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x =>
                {
                    if (x is PetErrorBadRequest)
                    {
                        return BadRequest(x.Message);
                    }
                    else return NotFound(x.Message);
                }
                );
        }

        [HttpPut, Authorize(Roles = "ADMIN,VET")]
        public ActionResult UpdatePet([FromBody] PetDTOupdate dto)
        {
            var err = dto.Validate();
            if (err != null) return BadRequest(err);

            var task = Service.Update(dto);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN,VET")]
        public ActionResult DeletePet(Guid id)
        {
            var task = Service.Delete(id);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x =>
                {
                    if (x is PetErrorBadRequest)
                    {
                        return BadRequest(x.Message);
                    }
                    else return NotFound(x.Message);
                }
                );
        }
    }
}
