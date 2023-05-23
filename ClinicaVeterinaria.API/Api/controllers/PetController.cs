﻿using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.API.Api.controllers
{
    [ApiController]
    [Route("pets")]
    public class PetController
    {
        private readonly PetService Service;

        public PetController(PetService service)
        {
            Service = service;
        }

        [HttpGet, Authorize(Roles = "ADMIN,VET")]
        public IResult FindAllPets()
        {
            var task = Service.FindAll();
            task.Wait();

            return Results.Ok(task.Result);
        }

        [HttpGet("{id}"), Authorize(Roles = "ADMIN,VET,USER")]
        public IResult FindPetById(Guid id)
        {
            var task = Service.FindById(id);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpPost, Authorize(Roles = "ADMIN,VET")]
        public IResult CreatePet([FromBody] PetDTOcreate dto)
        {
            var err = dto.Validate();
            if (err != null) return Results.BadRequest(err);

            var task = Service.Create(dto);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x =>
                {
                    if (x is PetErrorBadRequest)
                    {
                        return Results.BadRequest(x.Message);
                    }
                    else return Results.NotFound(x.Message);
                }
                );
        }

        [HttpPut, Authorize(Roles = "ADMIN,VET")]
        public IResult UpdatePet([FromBody] PetDTOupdate dto)
        {
            var err = dto.Validate();
            if (err != null) return Results.BadRequest(err);

            var task = Service.Update(dto);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x => Results.NotFound(x)
                );
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN,VET")]
        public IResult DeletePet(Guid id)
        {
            var task = Service.Delete(id);
            task.Wait();

            return task.Result.Match
                (
                onSuccess: x => Results.Ok(x),
                onError: x =>
                {
                    if (x is PetErrorBadRequest)
                    {
                        return Results.BadRequest(x.Message);
                    }
                    else return Results.NotFound(x.Message);
                }
                );
        }
    }
}
