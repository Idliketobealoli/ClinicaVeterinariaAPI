using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class HistoryMapper
    {
        public static HistoryDTO ToDTO(this History history)
        {
            HashSet<VaccineDTO> vaccines = new();
            foreach (Vaccine v in history.Vaccines)
            {
                var newVaccine = v.ToDTO();
                vaccines.Add(newVaccine);
            }

            return new
                (
                history.PetId,
                vaccines,
                history.AilmentTreatment
                );
        }

        public static HistoryDTOvaccines ToDTOvaccines(this History history)
        {
            HashSet<VaccineDTO> vaccines = new();
            foreach (Vaccine v in history.Vaccines)
            {
                var newVaccine = v.ToDTO();
                vaccines.Add(newVaccine);
            }

            return new(vaccines);
        }

        public static HistoryDTOailmentTreatment ToDTOailmentTreatment(this History history)
        {
            return new(history.AilmentTreatment);
        }
    }
}
