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
                user.Phone
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
                CipherService.Encode(dto.Password)
                );
        }

        public static UserDTOandToken toDTOwithToken(this User user)
        {
            return new
                (
                user.ToDTO(),
                "token" // esto habrá que modificarlo cuando metamos tokens
                );
        }
    }
}
