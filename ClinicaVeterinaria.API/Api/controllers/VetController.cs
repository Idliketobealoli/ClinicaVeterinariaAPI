using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("vets")]
    public class VetController: ControllerBase
    {
        private readonly VetService Service;
        private readonly IConfiguration _configuration;

        public VetController(VetService service, IConfiguration configuration)
        {
            Service = service;
            _configuration = configuration;
        }

        /// <summary>
        /// Finds all vets in the database.
        /// </summary>
        /// <returns>
        /// A list containing all vets in the database.
        /// </returns>
        /// <response code="200" />
        [HttpGet, Authorize(Roles = "ADMIN")]
        public ActionResult FindAllVets()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

        /// <summary>
        /// Finds the vet whose email corresponds with the one given.
        /// </summary>
        /// <returns>
        /// The vet, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("{email}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindVetByEmail(string email)
        {
            var task = Service.FindByEmail(email);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Finds the vet whose email corresponds with the one given, but returns a shortened version of it.
        /// </summary>
        /// <returns>
        /// The shortened vet, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("short/{email}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindVetByEmailShort(string email)
        {
            var task = Service.FindByEmailShort(email);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Finds the vet whose email corresponds with the one given, but returns just the information required for an appointment.
        /// </summary>
        /// <returns>
        /// The vet's information that is needed for an appointment, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("appointment/{email}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindVetByEmailAppointment(string email)
        {
            var task = Service.FindByEmailAppointment(email);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Registers a vet.
        /// </summary>
        /// <returns>
        /// The vet and a token, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
        [HttpPost("register")]
        public ActionResult RegisterVet([FromBody] VetDTOregister dto)
        {
            var err = dto.Validate();
            if (err != null) return BadRequest(err);

            var task = Service.Register(dto, _configuration);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => BadRequest(x)
                );
        }

        /// <summary>
        /// Lets a vet log in.
        /// </summary>
        /// <returns>
        /// The vet and a token, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
        [HttpPost("login")]
        public ActionResult LoginVet([FromBody] VetDTOloginOrChangePassword dto)
        {
            var err = dto.Validate();
            if (err != null) return BadRequest(err);

            var task = Service.Login(dto, _configuration);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => BadRequest(x)
                );
        }

        /// <summary>
        /// Lets a vet change its password.
        /// </summary>
        /// <returns>
        /// The vet, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
        [HttpPut, Authorize(Roles = "ADMIN,VET")]
        public ActionResult ChangeVetPassword([FromBody] VetDTOloginOrChangePassword dto)
        {
            var err = dto.Validate();
            if (err != null) return BadRequest(err);

            var task = Service.ChangePassword(dto);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Deletes the vet whose email corresponds with the one given.
        /// </summary>
        /// <returns>
        /// The vet, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpDelete("{email}"), Authorize(Roles = "ADMIN")]
        public ActionResult DeleteVet(string email)
        {
            var task = Service.Delete(email);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        /// <summary>
        /// Finds the vet whose email corresponds with the one in the token.
        /// </summary>
        /// <returns>
        /// The vet, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
        [HttpGet("me"), Authorize(Roles = "ADMIN,VET")]
        public ActionResult MeVet()
        {
            var vetEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var task = Service.FindByEmail(vetEmail);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }
    }
}
