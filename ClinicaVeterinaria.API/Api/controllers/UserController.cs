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

        /// <summary>
        /// Finds all users present in the database.
        /// </summary>
        /// <returns>
        /// A list containing all users in the database.
        /// </returns>
        /// <response code="200" />
        [HttpGet, Authorize(Roles = "ADMIN")]
        public ActionResult FindAllUsers()
        {
            var task = Service.FindAll();
            task.Wait();

            return Ok(task.Result);
        }

        /// <summary>
        /// Finds the user whose email corresponds with the one given.
        /// </summary>
        /// <returns>
        /// The user, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
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

        /// <summary>
        /// Finds the user whose email corresponds with the one given, but returns a shortened version of the user information.
        /// </summary>
        /// <returns>
        /// The shortened user, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
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

        /// <summary>
        /// Registers the user in the database.
        /// </summary>
        /// <returns>
        /// The user and a token, or a bad request error if it could not be added.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
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

        /// <summary>
        /// Lets the user log in.
        /// </summary>
        /// <returns>
        /// The user and a token, or a bad request error.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
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

        /// <summary>
        /// Lets the user change its password.
        /// </summary>
        /// <returns>
        /// The user, or an error response.
        /// </returns>
        /// <response code="200" />
        /// <response code="400" />
        /// <response code="404" />
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

        /// <summary>
        /// Deletes the user whose email corresponds with the one given.
        /// </summary>
        /// <returns>
        /// The user, or a not found error.
        /// </returns>
        /// <response code="200" />
        /// <response code="404" />
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
