using ClinicaVeterinaria.API.Api.dto;

namespace ClinicaVeterinaria.API.Api.validators
{
    public static class AilmentTreatmentValidator
    {
        public static string? Validate(this AilmentTreatmentDTO dto)
        {
            if (dto == null) return "Ailment-Treatment is null.";

            else if (!dto.Ailment.Trim().Any())
                return "Ailment must not be null or blank.";

            else if (dto.Ailment.Trim().Length < 2)
                return "Ailment must not be a single letter.";

            else if (!dto.Treatment.Trim().Any())
                return "Ailment must not be null or blank.";

            else if (dto.Treatment.Trim().Length < 2)
                return "Ailment must not be a single letter.";

            else return null;
        }
    }
}
