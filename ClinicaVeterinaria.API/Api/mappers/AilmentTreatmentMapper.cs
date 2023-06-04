using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class AilmentTreatmentMapper
    {
        public static AilmentTreatmentDTO ToDTO(this AilmentTreatment at)
        {
            return new
                (
                at.Ailment,
                at.Treatment
                );
        }

        public static AilmentTreatment FromDTO(this AilmentTreatmentDTO dto, Guid petId)
        {
            return new
                (
                petId,
                dto.Ailment,
                dto.Treatment
                );
        }
    }
}
