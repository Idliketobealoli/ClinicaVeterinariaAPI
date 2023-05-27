using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class VaccineMapper
    {
        public static VaccineDTO ToDTO(this Vaccine vaccine)
        {
            return new
                (
                vaccine.Name,
                vaccine.Date.ToString()
                );
        }

        public static Vaccine FromDTO(this VaccineDTO dto, Guid petId)
        {
            return new
                (
                petId,
                dto.Name,
                DateOnly.Parse(dto.Date)
                );
        }
    }
}
