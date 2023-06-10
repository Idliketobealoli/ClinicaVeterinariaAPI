using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.services.bcrypt;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class VetMapper
    {
        public static VetDTO ToDTO(this Vet vet)
        {
            return new
                (
                vet.Name,
                vet.Surname,
                vet.Email,
                vet.SSNumber,
                vet.Role,
                vet.Specialty,
                vet.Active
                );
        }

        public static VetDTOshort ToDTOshort(this Vet vet)
        {
            return new
                (
                vet.Name,
                vet.Surname
                );
        }

        public static VetDTOstats ToDTOstats(this Vet vet, int amount)
        {
            return new
                (
                vet.Name,
                vet.Surname,
                vet.Email,
                amount
                );
        }

        public static VetDTOappointment ToDTOappointment(this Vet vet)
        {
            return new
                (
                vet.Name,
                vet.Surname,
                vet.Email,
                vet.Active
                );
        }

        public static Vet FromDTOregister(this VetDTOregister dto)
        {
            return new
                (
                dto.Name,
                dto.Surname,
                dto.Email,
                dto.SSNumber,
                CipherService.Encode(dto.Password),
                Roles.FromString(dto.Role),
                dto.Specialty,
                true
                );
        }

        public static VetDTOandToken ToDTOwithToken(this Vet vet, string? token)
        {
            return new
                (
                vet.ToDTO(),
                token ?? ""
                );
        }
    }
}
