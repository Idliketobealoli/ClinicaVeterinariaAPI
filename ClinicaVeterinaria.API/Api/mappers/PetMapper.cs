﻿using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class PetMapper
    {
        public static PetDTOshort ToDTOshort(this Pet pet)
        {
            return new
                (
                pet.Id,
                pet.Name,
                pet.Race,
                pet.Species,
                Sexes.ToString(pet.Sex)
                );
        }

        public static PetDTO ToDTO(this Pet pet, User owner, VaccineRepository vRepo, AilmentTreatmentRepository aRepo)
        {
            return new
                (
                pet.Id,
                pet.Name,
                pet.Race,
                pet.Species,
                Sexes.ToString(pet.Sex),
                pet.BirthDate.ToString(),
                pet.Weight,
                pet.Size,
                pet.History.ToDTO(vRepo, aRepo),
                owner.ToDTOshort(),
                pet.Active
                );
        }

        public static Pet FromDTO(this PetDTOcreate dto)
        {
            return new
                (
                Guid.NewGuid(),
                dto.Name,
                dto.Species,
                dto.Race,
                dto.Weight,
                dto.Size,
                Sexes.FromString(dto.Sex),
                DateOnly.Parse(dto.Date),
                dto.OwnerEmail,
                true
                );
        }
    }
}
