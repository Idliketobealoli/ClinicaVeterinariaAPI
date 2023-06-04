using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.services.bcrypt;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class UserMapper
    {
        public static UserDTO ToDTO(this User user)
        {
            return new
                (
                user.Name,
                user.Surname,
                user.Email,
                user.Phone,
                user.Active
                );
        }

        public static UserDTOshort ToDTOshort(this User user)
        {
            return new
                (
                user.Name,
                user.Surname
                );
        }

        public static User FromDTOregister(this UserDTOregister dto)
        {
            return new
                (
                dto.Name,
                dto.Surname,
                dto.Email,
                dto.Phone,
                CipherService.Encode(dto.Password),
                true
                );
        }

        public static UserDTOandToken ToDTOwithToken(this User user, string? token)
        {
            return new
                (
                user.ToDTO(),
                token ?? ""
                );
        }
    }
}
