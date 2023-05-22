using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("users")]
    public class UserController
    {
        private readonly UserService Service;
        private readonly IConfiguration _configuration;

        public UserController(UserService service, IConfiguration configuration)
        {
            Service = service;
            _configuration = configuration;
        }

        [HttpGet, Authorize]
        public IResult FindAllUsers()
        {
            var task = Service.FindAll();
            task.Wait();

            return Results.Ok(task.Result);
        }

        [HttpGet("{email}"), Authorize]
        public IResult FindUserByEmail(string email)
        {
            var task = Service.FindByEmail(email);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpGet("short/{email}"), Authorize]
        public IResult FindUserByEmailShort(string email)
        {
            var task = Service.FindByEmailShort(email);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpPost("register")]
        public IResult RegisterUser([FromBody] UserDTOregister dto)
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
        public IResult LoginUser([FromBody] UserDTOloginOrChangePassword dto)
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

        [HttpPut, Authorize]
        public IResult ChangeUserPassword([FromBody] UserDTOloginOrChangePassword dto)
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

        [HttpDelete("{email}"), Authorize]
        public IResult DeleteUser(string email)
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
