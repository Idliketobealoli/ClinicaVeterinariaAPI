using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet, Authorize(Roles = "ADMIN")]
        public ActionResult FindAllVets()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

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
    }
}
