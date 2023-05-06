using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using System.Net.Mail;

namespace ClinicaVeterinaria.API.Api.validators
{
    public static class UserValidator
    {
        public static string? Validate(this UserDTOregister dto)
        {
            if (dto == null) return "Data must not be null.";

            else if (!dto.Name.Trim().Any())
                return "Name must not be null or blank.";

            else if (dto.Name.Trim().Length < 2)
                return "Name must not be a single letter.";

            else if (!dto.Surname.Trim().Any())
                return "Surname must not be null or blank.";

            else if (dto.Surname.Trim().Length < 2)
                return "Surname must not be a single letter.";

            else if (!MailAddress.TryCreate(dto.Email.Trim(), out _))
                return "Incorrect email address expression.";

            else if (dto.Phone.Trim().Length < 9)
                return "Phone number too short to be correct.";

            else if (dto.Password.Trim() != dto.RepeatPassword.Trim())
                return "Passwords do not match.";
            else return null;
        }

        public static string? Validate(this UserDTOloginOrChangePassword dto)
        {
            if (dto == null) return "Data must not be null.";

            else if (!MailAddress.TryCreate(dto.Email.Trim(), out _))
                return "Incorrect email address expression.";

            else if (dto.Password.Length < 7)
                return "Password must be at least 7 characters long.";
            else return null;
        }
    }
}