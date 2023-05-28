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

        /// <summary>
        /// Finds all pets stored in the database.
        /// </summary>
        /// <returns>
        /// A list containing all pets.
        /// </returns>
        /// <response code="200" />
        [HttpGet, Authorize(Roles = "ADMIN,VET")]
        public ActionResult FindAllPets()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

        /// <summary>
        /// Finds a pet with the corresponing ID.
        /// </summary>
        /// <returns>
        /// The pet, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
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

        /// <summary>
        /// Creates a pet.
        /// </summary>
        /// <returns>
        /// The pet, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
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

        /// <summary>
        /// Updates the pet with the same ID as the one in the body.
        /// </summary>
        /// <returns>
        /// The history, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
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

        /// <summary>
        /// Deletes the pet whose ID matches the one given.
        /// </summary>
        /// <returns>
        /// The pet, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
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
