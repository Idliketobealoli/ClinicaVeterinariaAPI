using ClinicaVeterinaria.API.Api.dto;
using System.Net.Mail;

namespace ClinicaVeterinaria.API.Api.validators
{
    public static class AppointmentValidator
    {
        public static string? Validate(this AppointmentDTOcreate dto)
        {
            if (dto == null) return "Data must not be null.";

            else if (!MailAddress.TryCreate(dto.UserEmail.Trim(), out _))
                return "Incorrect user email address expression.";

            else if (!MailAddress.TryCreate(dto.VetEmail.Trim(), out _))
                return "Incorrect vet email address expression.";

            else if (DateOnly.TryParse(dto.InitialDate, out DateOnly dt))
            {
                if (dt > DateOnly.FromDateTime(DateTime.Now))
                    return "Initial date must not be in the future.";
                else if (DateOnly.TryParse(dto.InitialDate, out DateOnly dt2)) {
                    if (dt2 > DateOnly.FromDateTime(DateTime.Now))
                        return "Finish date must not be in the future.";
                    else return null;
                }
                else return "Finsih date must be in a valid date format.";
            }
            else return "Initial date must be in a valid date format.";
        }
    }
}
