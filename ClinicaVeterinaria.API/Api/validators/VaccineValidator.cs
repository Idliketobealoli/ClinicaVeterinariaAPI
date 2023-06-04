using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using System.Net.Mail;

namespace ClinicaVeterinaria.API.Api.validators
{
    public static class VaccineValidator
    {
        public static string? Validate(this VaccineDTO dto)
        {
            if (dto == null) return "Vaccine is null.";

            else if (!dto.Name.Trim().Any())
                return "Vaccine name must not be null or blank.";

            else if (dto.Name.Trim().Length < 2)
                return "Vaccine name must not be a single letter.";

            else
            {
                if (DateOnly.TryParse(dto.Date, out DateOnly dt))
                {
                    if (dt > DateOnly.FromDateTime(DateTime.Now))
                        return "Vaccine addition must not be in the future.";

                    else return null;
                }
                else return "Vaccine addition must be in a valid date format.";
            }
        }
    }
}
