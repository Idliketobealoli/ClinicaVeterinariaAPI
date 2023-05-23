using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("vets")]
    public class VetController
    {
        private readonly VetService Service;
        private readonly IConfiguration _configuration;

        public VetController(VetService service, IConfiguration configuration)
        {
            Service = service;
            _configuration = configuration;
        }

        [HttpGet, Authorize(Roles = "ADMIN")]
        public IResult FindAllVets()
        {
            var task = Service.FindAll();
            task.Wait();

            return Results.Ok(task.Result);
        }

        [HttpGet("{email}"), Authorize(Roles = "ADMIN,VET,USER")]
        public IResult FindVetByEmail(string email)
        {
            var task = Service.FindByEmail(email);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpGet("short/{email}"), Authorize(Roles = "ADMIN,VET,USER")]
        public IResult FindVetByEmailShort(string email)
        {
            var task = Service.FindByEmailShort(email);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpGet("appointment/{email}"), Authorize(Roles = "ADMIN,VET,USER")]
        public IResult FindVetByEmailAppointment(string email)
        {
            var task = Service.FindByEmailAppointment(email);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpPost("register")]
        public IResult RegisterVet([FromBody] VetDTOregister dto)
        {
            var err = dto.Validate();
            if (err != null) return Results.BadRequest(err);

            var task = Service.Register(dto, _configuration);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.BadRequest(x)
                );
        }

        [HttpPost("login")]
        public IResult LoginVet([FromBody] VetDTOloginOrChangePassword dto)
        {
            var err = dto.Validate();
            if (err != null) return Results.BadRequest(err);

            var task = Service.Login(dto, _configuration);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.BadRequest(x)
                );
        }

        [HttpPut, Authorize(Roles = "ADMIN,VET")]
        public IResult ChangeVetPassword([FromBody] VetDTOloginOrChangePassword dto)
        {
            var err = dto.Validate();
            if (err != null) return Results.BadRequest(err);

            var task = Service.ChangePassword(dto);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpDelete("{email}"), Authorize(Roles = "ADMIN")]
        public IResult DeleteVet(string email)
        {
            var task = Service.Delete(email);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }
    }
}
