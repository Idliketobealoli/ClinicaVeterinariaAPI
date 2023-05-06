using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;

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
                pet.Sex
                );
        }

        public static PetDTO ToDTO(this Pet pet, User owner)
        {
            return new
                (
                pet.Id,
                pet.Name,
                pet.Race,
                pet.Species,
                pet.Sex,
                pet.BirthDate,
                pet.Weight,
                pet.Size,
                pet.History.ToDTO(),
                owner.ToDTOshort()
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
                dto.Sex,
                dto.Date,
                dto.OwnerEmail
                );
        }
    }
}
