using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("users")]
    public class UserController: ControllerBase
    {
        private readonly UserService Service;
        private readonly IConfiguration _configuration;

        public UserController(UserService service, IConfiguration configuration)
        {
            Service = service;
            _configuration = configuration;
        }

        [HttpGet, Authorize(Roles = "ADMIN")]
        public ActionResult FindAllUsers()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

        [HttpGet("{email}"), Authorize(Roles = "ADMIN,VET,USER")]
        public ActionResult FindUserByEmail(string email)
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
        public ActionResult FindUserByEmailShort(string email)
        {
            var task = Service.FindByEmailShort(email);
            task.Wait();

            return task.Result.Match<ActionResult>
                (
                onSuccess: x => Ok(x),
                onError: x => NotFound(x)
                );
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] UserDTOregister dto)
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
        public ActionResult LoginUser([FromBody] UserDTOloginOrChangePassword dto)
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

        [HttpPut, Authorize(Roles = "USER")]
        public ActionResult ChangeUserPassword([FromBody] UserDTOloginOrChangePassword dto)
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

        [HttpDelete("{email}"), Authorize(Roles = "ADMIN,USER")]
        public ActionResult DeleteUser(string email)
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
