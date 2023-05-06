using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;

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

            else return null;
        }
    }
}
