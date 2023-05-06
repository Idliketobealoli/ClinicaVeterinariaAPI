using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("vets")]
    public class VetController
    {
        private readonly VetService Service;

        public VetController(VetService service)
        {
            Service = service;
        }

        [HttpGet]
        public IResult FindAllVets()
        {
            Stopwatch.StartNew();
            var task = Service.FindAll();
            task.Wait();
            var time = Stopwatch.GetTimestamp();

            return Results.Ok(task.Result);
        }

        [HttpGet("{email}")]
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

        [HttpGet("short/{email}")]
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

        [HttpGet("appointment/{email}")]
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

            var task = Service.Register(dto);
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

            var task = Service.Login(dto);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.BadRequest(x)
                );
        }

        [HttpPut]
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

        [HttpDelete("{email}")]
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
