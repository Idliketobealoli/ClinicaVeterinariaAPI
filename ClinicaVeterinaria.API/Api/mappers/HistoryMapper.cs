using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class HistoryMapper
    {
        public static HistoryDTO ToDTO(this History history, VaccineRepository vRepo, AilmentTreatmentRepository aRepo)
        {
            HashSet<VaccineDTO> vaccines = new();
            var vaccinesFromDB = vRepo.FindByPetId(history.PetId);
            vaccinesFromDB.Wait();
            if (vaccinesFromDB.Result != null)
            {
                foreach (Vaccine v in vaccinesFromDB.Result)
                {
                    var newVaccine = v.ToDTO();
                    vaccines.Add(newVaccine);
                }
            }

            HashSet<AilmentTreatmentDTO> ats = new();
            var atsFromDB = aRepo.FindByPetId(history.PetId);
            atsFromDB.Wait();
            if (atsFromDB.Result != null)
            {
                foreach (AilmentTreatment at in atsFromDB.Result)
                {
                    var newAT = at.ToDTO();
                    ats.Add(newAT);
                }
            }

            return new
                (
                history.PetId,
                vaccines,
                ats
                );
        }

        public static HistoryDTOvaccines ToDTOvaccines(this History history, VaccineRepository vRepo)
        {
            HashSet<VaccineDTO> vaccines = new();
            var vaccinesFromDB = vRepo.FindByPetId(history.PetId);
            vaccinesFromDB.Wait();
            if (vaccinesFromDB.Result != null)
            {
                foreach (Vaccine v in vaccinesFromDB.Result)
                {
                    var newVaccine = v.ToDTO();
                    vaccines.Add(newVaccine);
                }
            }

            return new(vaccines);
        }

        public static HistoryDTOailmentTreatment ToDTOailmentTreatment(this History history, AilmentTreatmentRepository aRepo)
        {
            HashSet<AilmentTreatmentDTO> ats = new();
            var atsFromDB = aRepo.FindByPetId(history.PetId);
            atsFromDB.Wait();
            if (atsFromDB.Result != null)
            {
                foreach (AilmentTreatment at in atsFromDB.Result)
                {
                    var newAT = at.ToDTO();
                    ats.Add(newAT);
                }
            }
            return new(ats);
        }
    }
}
